<template>
  <section class="overflow-hidden rounded-2xl border border-[#e8e9e2] bg-white text-[#263a30] shadow-sm">
    <header class="border-b border-[#ecece6] px-5 py-6 sm:px-8">
      <div class="flex flex-wrap items-end justify-between gap-5">
        <div>
          <h2 class="mt-1 font-display text-2xl font-semibold text-[#20392d]">{{ $t('users') }}</h2>
        </div>
        <button class="rounded-lg bg-[#315d45] px-5 py-2.5 text-sm font-semibold text-white transition hover:bg-[#264b38]" @click="openCreateModal">{{ $t('create') }}</button>
      </div>
      <div class="mt-6 grid grid-cols-1 gap-4 rounded-xl bg-[#f3f5f0] p-4 sm:grid-cols-3 sm:p-5">
        <div><p class="text-2xl font-semibold text-[#253d32]">{{ pagination.totalCount }}</p><p class="mt-1 text-sm text-[#727e73]">{{ $t('users') }}</p></div>
        <div class="sm:border-l sm:border-[#dfe4da] sm:pl-6"><p class="text-2xl font-semibold text-[#253d32]">{{ selectedIds.length }}</p><p class="mt-1 text-sm text-[#727e73]">{{ $t('selected') }}</p></div>
        <div class="flex items-center justify-between gap-3 sm:border-l sm:border-[#dfe4da] sm:pl-6">
          <input id="search" v-model="searchValue" type="text" :placeholder="$t('searching')" class="w-full rounded-full border border-[#d9ddd5] bg-white px-4 py-2 text-sm text-[#263a30] placeholder:text-[#92998f] focus:border-[#6b8b70] focus:outline-none focus:ring-2 focus:ring-[#6b8b70]/20">
          <button v-if="selectedIds.length" class="shrink-0 rounded-lg bg-red-600 px-3 py-2 text-sm font-medium text-white hover:bg-red-700" @click="openDeleteModal"><i class="fas fa-trash mr-2" />{{ $t('delete') }}</button>
        </div>
      </div>
    </header>

    <div v-if="users.length" class="overflow-x-auto">
      <table class="w-full min-w-[760px] text-left text-sm">
        <thead class="text-xs font-semibold uppercase tracking-wide text-[#818a80]">
          <tr class="border-b border-[#ecece6]">
            <th class="w-12 px-5 py-4"><span class="sr-only">{{ $t('selected') }}</span></th>
            <th class="px-4 py-4">{{ $t('user') }}</th>
            <th class="px-4 py-4">{{ $t('roles') }}</th>
            <th class="px-4 py-4">{{ $t('income') }}</th>
            <th class="px-5 py-4 text-right">{{ $t('actions') }}</th>
          </tr>
        </thead>
        <tbody>
          <tr v-for="user in users" :key="user.id" class="border-b border-[#f0f1ec] transition hover:bg-[#fafbf8]">
            <td class="px-5 py-4"><input type="checkbox" :checked="selectedIds.includes(user.id)" class="h-4 w-4 rounded border-[#cbd2c8] accent-[#315d45]" @change="toggleSelection(user.id)"></td>
            <td class="px-4 py-4">
              <div class="flex min-w-0 items-center gap-3">
                <button type="button" class="group relative h-10 w-10 shrink-0 cursor-pointer overflow-hidden rounded-full bg-[#e8eee5]" :title="$t('change_profile_picture')" @click.stop="openProfilePicturePicker(user.id)">
                  <img :src="getProfilePicture(user)" :alt="user.lastName + ' ' + user.firstName" class="h-full w-full object-cover">
                  <span class="absolute inset-0 flex items-center justify-center bg-black/45 text-white opacity-0 transition group-hover:opacity-100"><i class="fas fa-camera text-xs" /></span>
                </button>
                <input :id="`profile-picture-${user.id}`" class="sr-only" type="file" accept="image/*" @change="changeProfilePicture($event, user)">
                <button class="min-w-0 text-left" @click="openEditModal(user)">
                  <span class="block truncate font-semibold text-[#293b30]">{{ user.lastName }} {{ user.firstName }}</span>
                  <span class="block truncate text-xs text-[#828a80]">{{ user.email }}<template v-if="user.nickName"> · {{ user.nickName }}</template></span>
                </button>
              </div>
            </td>
            <td class="px-4 py-4"><div class="flex flex-wrap gap-1.5"><span v-for="role in user.roles || []" :key="role" class="rounded-full bg-[#eef2eb] px-2.5 py-1 text-xs font-medium text-[#526d55]">{{ $t(getUserRoleLabelKey(role)) }}</span><span v-if="!user.roles?.length" class="text-[#92998f]">—</span></div></td>
            <td class="px-4 py-4 font-medium text-[#3f6b4d]">{{ formatCurrency(calculateTotalIncome(user)) }}</td>
            <td class="px-5 py-4"><div class="flex justify-end gap-2"><button class="rounded-md px-3 py-2 text-xs font-semibold text-[#42644b] hover:bg-[#edf2ea]" @click="openEditModal(user)">{{ $t('edit') }}</button><button class="rounded-md px-3 py-2 text-xs font-semibold text-[#647067] hover:bg-[#f1f2ed]" @click="openChangePasswordModal(user)">{{ $t('change_password') }}</button></div></td>
          </tr>
        </tbody>
      </table>
    </div>
    <div v-else class="px-6 py-16 text-center">
      <i class="fas fa-users text-3xl text-[#aab2a8]" />
      <h3 class="mt-3 font-semibold text-[#33483a]">{{ $t('no_users_found') }}</h3>
      <p class="mt-1 text-sm text-[#828a80]">{{ $t('get_started_by_creating_user') }}</p>
    </div>

    <footer v-if="pagination.totalPages > 1" class="flex items-center justify-between border-t border-[#ecece6] px-5 py-4 sm:px-8">
      <button @click="changePage(-1)" :disabled="pagination.currentPage === 1" class="rounded-lg border border-[#dfe2dc] px-4 py-2 text-sm text-[#536157] hover:bg-[#f7f8f5] disabled:opacity-40">{{ $t('previous') }}</button>
      <span class="text-sm text-[#727e73]">{{ $t('page') }} {{ pagination.currentPage }} {{ $t('of') }} {{ pagination.totalPages }}</span>
      <button @click="changePage(1)" :disabled="pagination.currentPage === pagination.totalPages" class="rounded-lg border border-[#dfe2dc] px-4 py-2 text-sm text-[#536157] hover:bg-[#f7f8f5] disabled:opacity-40">{{ $t('next') }}</button>
    </footer>

    <UserCreateModal ref="UserCreateModalRef" @item-created="refreshUsers" />
    <UserEditModal ref="UserEditModalRef" @item-edited="refreshUsers" />
    <UserChangePasswordModal ref="UserChangePasswordModalRef" @item-edited="refreshUsers" />
    <UserDeleteModal ref="UserDeleteModalRef" @item-deleted="handleDelete" />
  </section>
</template>

<script lang="ts">
import { defineComponent } from 'vue'
import { storeToRefs } from 'pinia'
import { useUserStore } from '~/stores/UserStore'
import UserCreateModal from '@/components/modals/users/UserCreateModal.vue'
import UserEditModal from '@/components/modals/users/UserEditModal.vue'
import UserChangePasswordModal from '@/components/modals/users/UserChangePasswordModal.vue'
import UserDeleteModal from '@/components/modals/users/UserDeleteModal.vue'
import type { UserListItem } from '~/models/UserModel'
import { getUserRoleBadgeClass, getUserRoleLabelKey } from '~/core/userRoles'
import { getStoredProfileImage, storeProfileImage } from '~/utils/profileImages'

definePageMeta({
  middleware: 'auth',
})

export default defineComponent({
  name: 'UserPage',
  components: { UserCreateModal, UserEditModal, UserChangePasswordModal, UserDeleteModal },
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
      await Promise.all([getRoles(), refreshUsers()])
      setActivePageTitle('users')
      firstLoad.value = false
    })

    const userStore = useUserStore()
    const { users } = storeToRefs(userStore)
    const { getUsers, getRoles } = userStore
    const { $showNotification } = useNuxtApp()

    const getProfilePicture = (user: UserListItem) => user.profileImage || getStoredProfileImage(user.id) || '/images/budget-manager-logo.png'

    const openProfilePicturePicker = (userId: string) => {
      document.getElementById(`profile-picture-${userId}`)?.click()
    }

    const resizeProfilePicture = async (file: File) => {
      const image = await createImageBitmap(file)
      const maxSize = 256
      const scale = Math.min(1, maxSize / Math.max(image.width, image.height))
      const canvas = document.createElement('canvas')
      canvas.width = Math.max(1, Math.round(image.width * scale))
      canvas.height = Math.max(1, Math.round(image.height * scale))
      const context = canvas.getContext('2d')
      if (!context) throw new Error('Could not process image')
      context.drawImage(image, 0, 0, canvas.width, canvas.height)
      image.close()
      return canvas.toDataURL('image/jpeg', 0.82)
    }

    const changeProfilePicture = async (event: Event, user: UserListItem) => {
      const input = event.target as HTMLInputElement
      const file = input.files?.[0]
      input.value = ''
      if (!file) return
      if (!file.type.startsWith('image/') || file.size > 5 * 1024 * 1024) {
        $showNotification('error', t('profile_picture_upload_failed'))
        return
      }

      try {
        const profileImage = await resizeProfilePicture(file)
        const updated = await userStore.updateUser(user.id, {
          id: user.id,
          firstName: user.firstName,
          lastName: user.lastName,
          nickName: user.nickName,
          email: user.email,
          profileImage,
        })
        if (!updated) throw new Error('Could not save profile picture')
        user.profileImage = profileImage
        storeProfileImage(user.id, profileImage)
        $showNotification('success', t('profile_picture_updated'))
      } catch (error) {
        console.error('Could not update profile picture:', error)
        $showNotification('error', t('profile_picture_upload_failed'))
      }
    }

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
        if (searchValue.value) params.searchTerm = searchValue.value;

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

    const UserChangePasswordModalRef = ref<InstanceType<typeof UserChangePasswordModal> | null>(null)
    const openChangePasswordModal = (user: UserListItem) => {
      if (UserChangePasswordModalRef.value) {
        UserChangePasswordModalRef.value.open(user)
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

    // Income helpers
    const getCurrentMonthValue = () => {
      const today = new Date()
      const year = today.getFullYear()
      const month = String(today.getMonth() + 1).padStart(2, '0')
      return `${year}-${month}`
    }

    const normalizeIncomeMonth = (dateString?: string) => {
      if (!dateString) return ''
      return dateString.slice(0, 7)
    }

    const calculateTotalIncome = (user: UserListItem) => {
      if (!user.incomes || user.incomes.length === 0) return 0
      const currentMonth = getCurrentMonthValue()
      return user.incomes
        .filter(income => normalizeIncomeMonth(income.date) === currentMonth)
        .reduce((sum, income) => sum + (income.amount || 0), 0)
    }

    const formatCurrency = (amount: number) => {
      return new Intl.NumberFormat('hu-HU', {
        style: 'currency',
        currency: 'HUF',
        minimumFractionDigits: 0,
        maximumFractionDigits: 0
      }).format(amount)
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
      UserChangePasswordModalRef,
      openChangePasswordModal,
      UserDeleteModalRef,
      openDeleteModal,
      handleDelete,
      calculateTotalIncome,
      formatCurrency,
      getProfilePicture,
      openProfilePicturePicker,
      changeProfilePicture,
      getUserRoleBadgeClass,
      getUserRoleLabelKey,
    }

  },
})
</script>
