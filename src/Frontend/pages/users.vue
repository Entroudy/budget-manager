<template>
    <div class="rounded-lg bg-white dark:bg-gray-800 shadow-xl dark:text-white px-4 py-4 md:px-8 md:py-6">
      <CustomTable :title="{ top: $t('list_of'), bottom: $t('users') }" :headings="headings"
        :table-data="users" :pagination="pagination" :sort-count="sortCount"
        :search="true" :search-query="searchValue" :hoverText="hoverText" :sort-by="sortBy"
        @on-page-size-change="changePageSize" @on-search="onSearchChange" @on-data-selected="selectRows"
        @on-page-change="changePage" @on-table-sort="sortTable" @on-row-item-click="openEditModal"
        @on-row-item-edit="editRowItem">
        <template #main-action>
          <div class="flex space-x-4">
            <button class="btn btn-md bg-primary text-white capitalize-block" @click="openCreateModal">
              {{ $t('create') }}
            </button>
          </div>
        </template>
        <template v-slot:rowItem="slotProps">
          <template v-if="slotProps.heading == 'url'">
              <div v-if="slotProps.data[0].url != ''">
                <a :href="slotProps.data[0].url.startsWith('http') ? slotProps.data[0].url : `https://${slotProps.data[0].url}`"
                rel="noopener noreferrer" class="underline text-md font-bold" target="_blank" @click.stop><i
                  class="fas fa-link text-xl" style="color:#c8d414"></i></a>
              </div>
              <div v-else>
                
              </div>
            </template>
          <template v-if="slotProps.heading == 'expireDate'">
            {{ formatDate(slotProps.data[0].expireDate) }}
          </template>
          <template v-if="slotProps.heading == 'name'">
            <p class="font-bold">{{ slotProps.data[0].name }}</p>
          </template>
          <template v-if="slotProps.heading == 'alert'">
            <div v-if="slotProps.data[0].alert">
              <i class="fas fa-bell text-xl" style="color:#c8d414"></i>
            </div>
            <div v-else>
              
            </div>
          </template>
        </template>
        <template #bulk-actions>
          <div class="relative z-40 flex">
            <button class="btn btn-sm bg-gray-700 text-white capitalize-block" v-click-outside="handleDropdownClick"
              @click="showBulkActionDropdown = !showBulkActionDropdown">
              <span class="hidden md:flex">
                <i class="fas fa-bars"></i>
              </span>
              <span class="md:hidden">{{ $t('group_actions') }}</span>
            </button>
            <div :class="[showBulkActionDropdown ? '' : 'hidden']"
              class="absolute right-1/2 translate-x-1/2 z-50 mt-2 w-32 origin-top-right cursor-pointer rounded-md bg-white dark:bg-gray-800 shadow-lg ring-1 ring-black ring-opacity-5 focus:outline-none"
              role="menu" aria-orientation="vertical" aria-labelledby="menu-button" tabindex="-1">
              <div class="py-1" role="none">
                <span @click="openDeleteModal"
                  class="capitalize-block hover:bg-gray-100 hover:text-gray-900 block rounded-md px-4 py-2 text-sm dark:text-white"
                  role="menuitem" tabindex="-1" id="menu-item-2">{{ $t('delete') }}</span>
              </div>
            </div>
          </div>
        </template>
      </CustomTable>
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
  
      const showBulkActionDropdown = ref(false)
      const handleDropdownClick = () => {
        if (showBulkActionDropdown.value) {
          showBulkActionDropdown.value = false
        }
      }
  
      const showExtraBulkActionDropdown = ref(false)
      const handleExtraDropdownClick = () => {
        if (showExtraBulkActionDropdown.value) {
          showExtraBulkActionDropdown.value = false
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
  
  
      type EditRowItemData<T = string> = {
        id: string;
        key: keyof UserListItem;
        value: T;
      };
      const editRowItem = async (data: EditRowItemData) => {
        const itemToEdit = users.value.find((item: { id: string }) => item.id === data.id)
        if (itemToEdit && itemToEdit[data.key] != data.value) {
          itemToEdit[data.key] = data.value
          const response = await editUser(itemToEdit)
          if (response) {
            $showNotification('success', `${t('successful_save')}`)
          }
        }
      }
  
      /* TABLE ACTIONS */
  
      const selectedIds = ref([])
      const headings = ref([
        {
          name: 'Teljes név',
          key: 'lastName', //todo
          width: 'auto',
          dataType: 'text',
          editMode: false,
          classes: ''
        },
        {
          name: 'Becenév',
          key: 'nickName',
          width: 'auto',
          dataType: 'text',
          editMode: false,
          classes: ''
        },
        {
          name: 'E-mail fiók',
          key: 'email',
          width: 'auto',
          dataType: 'text',
          editMode: false,
          classes: ''
        },
      ])
      const pagination = ref({
        currentPage: 1,
        pageSize: 10,
        totalCount: 0,
        totalPages: 0
      })
      const activationState = ref(true)
      const sortBy = ref('')
      const ascending = ref(false)
      const sortCount = ref(0)
      const searchValue = ref('')
      const hoverText = `${t('name') + ',' + t('type')}`
  
  
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
  
      watch(activationState, async (newValue) => {
        if (!firstLoad.value) {
          await refreshUsers()
        }
      })
  
      const onSearchChange = (value: string) => {
        searchValue.value = value
      }
  
      const selectRows = (data: any) => {
        selectedIds.value = data
      }
  
      const changePage = async (pageNumber: number) => {
        pagination.value.currentPage += pageNumber
        await refreshUsers()
      }
  
      const changePageSize = async (pageSize: string) => {
        pagination.value.pageSize = parseInt(pageSize)
        pagination.value.currentPage = 1
        await refreshUsers()
      }
  
      const formatDate = (value) => {
        const date = new Date(value)
        const year = date.getFullYear()
        const month = date.toLocaleString('hu-HU', { month: 'long' })
        const day = date.getDate()
  
        return `${year}. ${month} ${day}.`
      }
  
      const sortTable = async (sortByValue: string) => {
        sortCount.value++
        if (sortCount.value % 3 === 0) {
          sortCount.value = 0
          sortBy.value = ''
          ascending.value = false
          await refreshUsers()
          return
        }
        sortBy.value = sortByValue
        ascending.value = !ascending.value
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
        activationState,
        showBulkActionDropdown,
        showExtraBulkActionDropdown,
        handleDropdownClick,
        handleExtraDropdownClick,
        users,
        formatDate,
        selectedIds,
        headings,
        pagination,
        sortBy,
        hoverText,
        ascending,
        sortCount,
        searchValue,
        refreshUsers,
        editRowItem,
        onSearchChange,
        selectRows,
        changePage,
        changePageSize,
        sortTable,
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
  