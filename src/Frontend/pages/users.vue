<template>
    <div class="rounded-lg bg-white dark:bg-gray-800 shadow-xl dark:text-white px-4 py-4 md:px-8 md:py-6">
      <!-- Top Bar -->
      <div class="flex items-center justify-between gap-4 mb-6">
        <!-- Left: Page Size -->
        
        <div class="flex items-center gap-2">
          <button
            class="btn btn-md bg-primary text-white capitalize-block px-6 py-2 hover:bg-primary-dark transition-colors"
            @click="openCreateModal"
          >
            {{ $t('create') }}
          </button>
        </div>

        <div class="flex items-center gap-2">
          <input
            id="search"
            v-model="searchValue"
            type="text"
            :placeholder="$t('searching')"
            class="px-3 py-2 border border-gray-300 dark:border-gray-600 rounded-md bg-white dark:bg-gray-700 text-sm w-64"
          >
        </div>
      </div>

      <!-- Users List -->
      <div class="mt-6">
        

        <div v-if="users.length > 0" class="space-y-2">
          <div
            v-for="user in users"
            :key="user.id"
            @click="openEditModal(user)"
            class="flex items-center justify-between p-4 border border-gray-200 dark:border-gray-700 rounded-md hover:bg-indigo-50 dark:hover:bg-gray-700 cursor-pointer transition-colors"
          >
            <div class="flex flex-col">
              <span class="font-bold text-base">{{ user.lastName }}</span>
              <span class="text-sm text-gray-600 dark:text-gray-400">{{ user.nickName }}</span>
              <span class="text-sm text-gray-500 dark:text-gray-500">{{ user.email }}</span>
            </div>
            <div class="flex items-center gap-2">
              <input
                type="checkbox"
                :checked="selectedIds.includes(user.id)"
                @click.stop="toggleSelection(user.id)"
                class="w-4 h-4"
              >
            </div>
          </div>
        </div>

        <div v-else class="text-center py-8 text-gray-500">
          {{ $t('no_data') }}
        </div>

        <h3 class="text-s mt-4">
          {{ $t('list_of') }} {{ $t('users') }} ({{ users.length }} {{ $t('of') }} {{ pagination.totalCount }})
        </h3>
      </div>

      <!-- Bulk Actions -->
      <div v-if="selectedIds.length > 0" class="mt-4 flex gap-2">
        <button
          @click="openDeleteModal"
          class="btn btn-sm bg-red-600 text-white px-4 py-2 rounded-md hover:bg-red-700 transition-colors"
        >
          {{ $t('delete') }} ({{ selectedIds.length }})
        </button>
      </div>

      <!-- Pagination -->
      <div v-if="pagination.totalPages > 1" class="mt-6 flex items-center justify-between">
        <button
          @click="changePage(-1)"
          :disabled="pagination.currentPage === 1"
          class="px-4 py-2 bg-gray-200 dark:bg-gray-700 rounded-md disabled:opacity-50 disabled:cursor-not-allowed hover:bg-gray-300 dark:hover:bg-gray-600 transition-colors"
        >
          {{ $t('previous') }}
        </button>

        <span class="text-sm">
          {{ $t('page') }} {{ pagination.currentPage }} {{ $t('of') }} {{ pagination.totalPages }}
        </span>

        <button
          @click="changePage(1)"
          :disabled="pagination.currentPage === pagination.totalPages"
          class="px-4 py-2 bg-gray-200 dark:bg-gray-700 rounded-md disabled:opacity-50 disabled:cursor-not-allowed hover:bg-gray-300 dark:hover:bg-gray-600 transition-colors"
        >
          {{ $t('next') }}
        </button>
      </div>

      <UserCreateModal ref="UserCreateModalRef" @item-created="refreshUsers" />
      <UserEditModal ref="UserEditModalRef" @item-edited="refreshUsers" />
      <UserDeleteModal ref="UserDeleteModalRef" @item-deleted="handleDelete" />
    </div>
  </template>
  
  <script lang="ts">
  import { defineComponent } from 'vue'
  import { storeToRefs } from 'pinia'
  import { useUserStore } from '~/stores/UserStore'
  import UserCreateModal from '@/components/modals/users/UserCreateModal.vue'
  import UserEditModal from '@/components/modals/users/UserEditModal.vue'
  import UserDeleteModal from '@/components/modals/users/UserDeleteModal.vue'
  import type { UserListItem } from '~/models/UserModel'
  
  definePageMeta({
    middleware: 'auth',
  })

  export default defineComponent({
    name: 'UserPage',
    components: { UserCreateModal, UserEditModal, UserDeleteModal },
    setup(props, ctx) {
  
      const firstLoad = ref(true)
  
      const { t } = useI18n();
  
      const toggleSelection = (userId: string) => {
        const index = selectedIds.value.indexOf(userId)
        if (index > -1) {
          selectedIds.value.splice(index, 1)
        } else {
          selectedIds.value.push(userId)
        }
      }
  
      const { setActivePageTitle } = useGlobalStore()
  
      const { $formatDatePickerValue } = useNuxtApp()
      onMounted(async () => {
        initializeStateFromQuery(route.query);
        await refreshUsers()
        setActivePageTitle(`${t('users')}`)
        firstLoad.value = false
      })
  
      const { users } = storeToRefs(useUserStore())
      const { getUsers } = useUserStore()
      const { $showNotification } = useNuxtApp()
  
      const router = useRouter()
      const route = useRoute()
  
      const initializeStateFromQuery = (query) => {
        pagination.value.currentPage = parseInt(query.page) || 1;
        pagination.value.pageSize = parseInt(query.pageSize) || 10;
        sortBy.value = query.sortBy || '';
        ascending.value = query.ascending === false;
        searchValue.value = query.search || '';
      };
  
      const refreshUsers = async () => {
        try {
          const query: any = {
            page: pagination.value.currentPage,
            pageSize: pagination.value.pageSize,
          };

          // Only add optional parameters if they have values
          if (sortBy.value) query.sortBy = sortBy.value;
          if (ascending.value !== false) query.ascending = ascending.value;
          if (searchValue.value) query.search = searchValue.value;

          router.push({ query });

          const params: any = {
            page_number: pagination.value.currentPage,
            page_size: pagination.value.pageSize,
          };

          // Add optional params if they exist
          if (sortBy.value) params.sort_By = sortBy.value;
          if (searchValue.value) params.search_value = searchValue.value;

          const data = await getUsers(params)
          if (data) {
            pagination.value = {
              currentPage: data.currentPage,
              pageSize: data.pageSize,
              totalCount: data.totalCount,
              totalPages: data.totalPages,
            }
          }
        } catch (error) { }
      }
  
      /* TABLE ACTIONS */

      const selectedIds = ref([])
      const pagination = ref({
        currentPage: 1,
        pageSize: 10,
        totalCount: 0,
        totalPages: 0
      })
      const sortBy = ref('')
      const ascending = ref(false)
      const searchValue = ref('')
  
  
      watch(searchValue, async (newValue) => {
        if (!firstLoad.value) {
          if (newValue.length >= 2) {
            await refreshUsers()
          }
          if (newValue.length === 0) {
            await refreshUsers()
          }
        }
      })
  
  
      const changePage = async (pageNumber: number) => {
        pagination.value.currentPage += pageNumber
        await refreshUsers()
      }
  
      const changePageSize = async (pageSize: string) => {
        pagination.value.pageSize = parseInt(pageSize)
        pagination.value.currentPage = 1
        await refreshUsers()
      }
  
  
      /* MODAL ACTIONS */
  
      const UserCreateModalRef = ref<InstanceType<typeof UserCreateModal> | null>(null);
      const openCreateModal = () => {
        if (UserCreateModalRef.value) {
          UserCreateModalRef.value.open();
        }
      }
  
      const UserEditModalRef = ref<InstanceType<typeof UserEditModal> | null>(null);
      const openEditModal = (user: UserListItem) => {
        if (UserEditModalRef.value) {
          UserEditModalRef.value.open(user);
        }
      }
  
      const UserDeleteModalRef = ref<InstanceType<typeof UserDeleteModal> | null>(null);
      const openDeleteModal = () => {
        if (UserDeleteModalRef.value) {
          UserDeleteModalRef.value.open(selectedIds.value);
        }
      }
  
      const handleDelete = async () => {
        await refreshUsers()
        if (pagination.value.currentPage > 1 && users.value.length === 0) {
          changePage(-1)
        }
      }
  
      return {
        users,
        selectedIds,
        pagination,
        searchValue,
        refreshUsers,
        toggleSelection,
        changePage,
        changePageSize,
        UserCreateModalRef,
        openCreateModal,
        UserEditModalRef,
        openEditModal,
        UserDeleteModalRef,
        openDeleteModal,
        handleDelete,
      }
  
    },
  })
  </script>
  