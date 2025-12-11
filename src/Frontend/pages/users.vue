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

    <!-- Users Grid -->
    <div class="mt-6">
      <div v-if="users.length > 0" class="space-y-6">
        <!-- User Cards Grid -->
        <div class="grid grid-cols-1 md:grid-cols-2 gap-4">
          <div
            v-for="user in users"
            :key="user.id"
            class="group relative bg-gradient-to-br from-white to-gray-50 dark:from-gray-700 dark:to-gray-750 rounded-xl border border-gray-200 dark:border-gray-600 shadow-sm hover:shadow-lg transition-all duration-300 overflow-hidden"
          >
            <!-- Card Background Accent -->
            <div class="absolute top-0 right-0 w-32 h-32 bg-primary/5 rounded-full blur-3xl group-hover:bg-primary/10 transition-all duration-300"></div>

            <!-- Card Content -->
            <div class="relative p-6">
              <div class="flex items-start justify-between gap-4">
                <!-- User Info -->
                <div
                  class="flex-1 cursor-pointer"
                  @click="openEditModal(user)"
                >
                  <!-- Avatar -->
                  <div class="flex items-start gap-4">
                    <div class="w-14 h-14 rounded-full bg-gradient-to-br from-primary to-primary-dark flex items-center justify-center text-white font-bold text-lg shadow-md flex-shrink-0">
                      {{ user.firstName?.charAt(0) }}{{ user.lastName?.charAt(0) }}
                    </div>

                    <div class="flex-1 min-w-0">
                      <!-- Name -->
                      <h3 class="text-lg font-semibold text-gray-900 dark:text-white mb-1 truncate group-hover:text-primary transition-colors">
                        {{ user.lastName }} {{ user.firstName }}
                      </h3>

                      <!-- Nickname -->
                      <div class="flex items-center gap-2 mb-2">
                        <i class="fas fa-user text-gray-400 text-xs"></i>
                        <span class="text-sm text-gray-600 dark:text-gray-300 font-medium">{{ user.nickName }}</span>
                      </div>

                      <!-- Email -->
                      <div class="flex items-center gap-2">
                        <i class="fas fa-envelope text-gray-400 text-xs"></i>
                        <span class="text-sm text-gray-500 dark:text-gray-400 truncate">{{ user.email }}</span>
                      </div>
                    </div>
                  </div>
                </div>

                <!-- Checkbox -->
                <div class="flex-shrink-0">
                  <input
                    type="checkbox"
                    :checked="selectedIds.includes(user.id)"
                    @click.stop="toggleSelection(user.id)"
                    class="w-5 h-5 rounded border-2 border-gray-300 dark:border-gray-500 text-primary focus:ring-2 focus:ring-primary focus:ring-offset-0 cursor-pointer transition-all hover:border-primary dark:hover:border-primary"
                  >
                </div>
              </div>

              <!-- Quick Actions (visible on hover) -->
              <div class="mt-4 pt-4 border-t border-gray-200 dark:border-gray-600 opacity-0 group-hover:opacity-100 transition-opacity duration-200">
                <button
                  @click="openEditModal(user)"
                  class="text-sm text-primary hover:text-primary-dark font-medium flex items-center gap-2 transition-colors"
                >
                  <i class="fas fa-edit"></i>
                  <span>{{ $t('edit') }}</span>
                </button>
              </div>
            </div>
          </div>
        </div>

        <!-- Stats and Bulk Actions Bar -->
        <div class="bg-gradient-to-r from-gray-50 to-white dark:from-gray-700 dark:to-gray-750 rounded-lg border border-gray-200 dark:border-gray-600 p-4">
          <div class="flex items-center justify-between flex-wrap gap-4">
            <!-- Stats -->
            <div class="flex items-center gap-2">
              <div class="w-8 h-8 rounded-lg bg-primary/10 flex items-center justify-center">
                <i class="fas fa-users text-primary text-sm"></i>
              </div>
              <span class="text-sm text-gray-700 dark:text-gray-300">
                <span class="font-semibold">{{ users.length }}</span> {{ $t('of') }} <span class="font-semibold">{{ pagination.totalCount }}</span> {{ $t('users') }}
              </span>
            </div>

            <!-- Bulk Actions -->
            <div v-if="selectedIds.length > 0" class="flex items-center gap-3">
              <div class="flex items-center gap-2 px-3 py-1.5 bg-primary/10 rounded-lg">
                <i class="fas fa-check-circle text-primary text-sm"></i>
                <span class="text-sm font-medium text-primary">{{ selectedIds.length }} {{ $t('selected') }}</span>
              </div>
              <button
                @click="openDeleteModal"
                class="flex items-center gap-2 px-4 py-2 bg-red-600 text-white rounded-lg hover:bg-red-700 transition-colors shadow-sm"
              >
                <i class="fas fa-trash text-sm"></i>
                <span class="text-sm font-medium">{{ $t('delete') }}</span>
              </button>
            </div>
          </div>
        </div>
      </div>

      <!-- Empty State -->
      <div v-else class="text-center py-16">
        <div class="w-24 h-24 mx-auto mb-4 rounded-full bg-gray-100 dark:bg-gray-700 flex items-center justify-center">
          <i class="fas fa-users text-4xl text-gray-400"></i>
        </div>
        <h3 class="text-lg font-medium text-gray-900 dark:text-white mb-2">{{ $t('no_users_found') }}</h3>
        <p class="text-sm text-gray-500 dark:text-gray-400">{{ $t('get_started_by_creating_user') }}</p>
      </div>
    </div>

    <!-- Pagination -->
    <div v-if="pagination.totalPages > 1" class="mt-6 flex items-center justify-between bg-white dark:bg-gray-750 rounded-lg border border-gray-200 dark:border-gray-600 p-4">
      <button
        @click="changePage(-1)"
        :disabled="pagination.currentPage === 1"
        class="flex items-center gap-2 px-4 py-2 bg-white dark:bg-gray-700 border border-gray-300 dark:border-gray-600 rounded-lg disabled:opacity-50 disabled:cursor-not-allowed hover:bg-gray-50 dark:hover:bg-gray-600 transition-colors shadow-sm"
      >
        <i class="fas fa-chevron-left text-sm"></i>
        <span class="text-sm font-medium">{{ $t('previous') }}</span>
      </button>

      <div class="flex items-center gap-2">
        <span class="text-sm text-gray-600 dark:text-gray-400">{{ $t('page') }}</span>
        <span class="px-3 py-1 bg-primary text-white rounded-lg text-sm font-semibold">{{ pagination.currentPage }}</span>
        <span class="text-sm text-gray-600 dark:text-gray-400">{{ $t('of') }}</span>
        <span class="px-3 py-1 bg-gray-100 dark:bg-gray-700 rounded-lg text-sm font-semibold">{{ pagination.totalPages }}</span>
      </div>

      <button
        @click="changePage(1)"
        :disabled="pagination.currentPage === pagination.totalPages"
        class="flex items-center gap-2 px-4 py-2 bg-white dark:bg-gray-700 border border-gray-300 dark:border-gray-600 rounded-lg disabled:opacity-50 disabled:cursor-not-allowed hover:bg-gray-50 dark:hover:bg-gray-600 transition-colors shadow-sm"
      >
        <span class="text-sm font-medium">{{ $t('next') }}</span>
        <i class="fas fa-chevron-right text-sm"></i>
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

    const toggleSelection = (applicationUserId: string) => {
      const index = selectedIds.value.indexOf(applicationUserId)
      if (index > -1) {
        selectedIds.value.splice(index, 1)
      } else {
        selectedIds.value.push(applicationUserId)
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
      selectedIds.value = []
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
