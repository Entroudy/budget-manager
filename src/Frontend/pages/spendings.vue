<template>
  <div class="rounded-lg bg-white dark:bg-brand-darkGray shadow-xl dark:text-white px-4 py-4 md:px-8 md:py-6">
    <!-- Filters Section -->
    <div class="mb-6">
      <div class="flex items-center justify-between mb-4">
        <button class="btn btn-md bg-primary text-white capitalize-block" @click="openCreateModal">
          {{ $t('create') }}
        </button>
      </div>

      <div class="grid grid-cols-1 md:grid-cols-4 gap-4">
        <!-- User Filter -->
        <div>
          <label class="text-sm font-medium mb-1 block">{{ $t('user') }}:</label>
          <select
            v-model="filters.userId"
            class="w-full px-3 py-2 border border-gray-300 rounded-md dark:bg-gray-800 dark:border-gray-600"
            @change="applyFilters"
          >
            <option value="">{{ $t('all_users') }}</option>
            <option v-for="user in users" :key="user.id" :value="user.id">
              {{ user.nickName || user.lastName }}
            </option>
          </select>
        </div>

        <!-- From Date Filter -->
        <div>
          <label class="text-sm font-medium mb-1 block">{{ $t('from_date') }}:</label>
          <input
            type="date"
            v-model="filters.fromDate"
            class="w-full px-3 py-2 border border-gray-300 rounded-md dark:bg-gray-800 dark:border-gray-600"
            @change="applyFilters"
          />
        </div>

        <!-- To Date Filter -->
        <div>
          <label class="text-sm font-medium mb-1 block">{{ $t('to_date') }}:</label>
          <input
            type="date"
            v-model="filters.toDate"
            class="w-full px-3 py-2 border border-gray-300 rounded-md dark:bg-gray-800 dark:border-gray-600"
            @change="applyFilters"
          />
        </div>

        <!-- Color Filter -->
        <div>
          <label class="text-sm font-medium mb-1 block">{{ $t('color') }}:</label>
          <input
            type="color"
            v-model="filters.color"
            class="w-full h-10 px-1 py-1 border border-gray-300 rounded-md dark:bg-gray-800 dark:border-gray-600"
            @change="applyFilters"
          />
        </div>
      </div>

      <!-- Clear Filters Button -->
      <div class="mt-4" v-if="hasActiveFilters">
        <button
          @click="clearAllFilters"
          class="btn btn-sm bg-gray-500 text-white"
        >
          {{ $t('clear_all_filters') }}
        </button>
      </div>
    </div>

    <!-- Spendings List -->
    <div v-if="spendings.length > 0" class="space-y-4">
      <div
        v-for="spending in spendings"
        :key="spending.id"
        class="border border-gray-200 dark:border-gray-700 rounded-lg p-4 hover:bg-gray-50 dark:hover:bg-gray-800 cursor-pointer transition-colors"
        @click="openEditModal(spending)"
      >
        <div class="flex items-center justify-between">
          <div class="flex items-center space-x-4 flex-1">
            <!-- Color Indicator -->
            <div
              v-if="spending.color"
              class="w-4 h-4 rounded-lg flex-shrink-0"
              :style="{ backgroundColor: spending.color }"
            ></div>

            <!-- Item Name -->
            <div class="flex-1">
              <h3 class="text-lg font-semibold">{{ spending.item }}</h3>
              <p v-if="spending.comment" class="text-sm text-gray-600 dark:text-gray-400 mt-1">
                {{ spending.comment }}
              </p>
            </div>

            <!-- Amount -->
            <div class="text-right">
              <p class="text-xl font-bold">{{ formatCurrency(spending.amount) }}</p>
              <p class="text-sm text-gray-600 dark:text-gray-400">
                <i :class="getPaymentMethodIcon(spending.paymentMethod)"></i>
                {{ getPaymentMethodText(spending.paymentMethod) }}
              </p>
            </div>

            <!-- Date -->
            <div class="text-right">
              <p class="text-sm text-gray-600 dark:text-gray-400">
                {{ formatDate(spending.date) }}
              </p>
            </div>

            <!-- Actions -->
            <div class="flex space-x-2">
              <button
                @click.stop="openEditModal(spending)"
                class="btn btn-sm bg-blue-500 text-white"
              >
                <i class="fas fa-edit"></i>
              </button>
              <button
                @click.stop="openDeleteModal(spending.id)"
                class="btn btn-sm bg-red-500 text-white"
              >
                <i class="fas fa-trash"></i>
              </button>
            </div>
          </div>
        </div>
      </div>
    </div>

    <!-- Empty State -->
    <div v-else class="text-center py-12">
      <p class="text-gray-500 dark:text-gray-400 text-lg">
        {{ $t('no_spendings_found') }}
      </p>
    </div>

    <!-- Pagination -->
    <div v-if="pagination.totalPages > 1" class="mt-6 flex justify-center items-center space-x-4">
      <button
        @click="changePage(-1)"
        :disabled="pagination.currentPage === 1"
        class="btn btn-sm bg-gray-500 text-white disabled:opacity-50"
      >
        {{ $t('previous') }}
      </button>
      <span class="text-sm">
        {{ $t('page') }} {{ pagination.currentPage }} {{ $t('of') }} {{ pagination.totalPages }}
      </span>
      <button
        @click="changePage(1)"
        :disabled="pagination.currentPage === pagination.totalPages"
        class="btn btn-sm bg-gray-500 text-white disabled:opacity-50"
      >
        {{ $t('next') }}
      </button>
    </div>

    <!-- Modals -->
    <SpendingCreateModal ref="SpendingCreateModalRef" @item-created="refreshSpendings" />
    <SpendingEditModal ref="SpendingEditModalRef" @item-edited="refreshSpendings" />
    <SpendingDeleteModal ref="SpendingDeleteModalRef" @item-deleted="handleDelete" />
  </div>
</template>

<script lang="ts">
import { defineComponent } from 'vue'
import { storeToRefs } from 'pinia'
import { useSpendingStore } from '~/stores/SpendingStore'
import { useUserStore } from '~/stores/UserStore'
import { useGlobalStore } from '~/stores/global'
import { PaymentMethod, type SpendingItemResponse } from '~/models/SpendingModel'
import SpendingCreateModal from '@/components/modals/spending/SpendingCreateModal.vue'
import SpendingEditModal from '@/components/modals/spending/SpendingEditModal.vue'
import SpendingDeleteModal from '@/components/modals/spending/SpendingDeleteModal.vue'

definePageMeta({
  middleware: 'auth',
})

export default defineComponent({
  name: 'SpendingPage',
  components: { SpendingCreateModal, SpendingEditModal, SpendingDeleteModal },
  setup() {
    const { t } = useI18n()
    const { setActivePageTitle } = useGlobalStore()
    const { spendings } = storeToRefs(useSpendingStore())
    const { getSpendings } = useSpendingStore()
    const { users } = storeToRefs(useUserStore())
    const { getUsers } = useUserStore()
    const router = useRouter()
    const route = useRoute()

    const filters = ref({
      userId: '',
      fromDate: '',
      toDate: '',
      color: ''
    })
    const firstLoad = ref(true)

    const pagination = ref({
      currentPage: 1,
      pageSize: 50,
      totalCount: 0,
      totalPages: 0
    })

    onMounted(async () => {
      setActivePageTitle('Spending')
      // Load users first
      await getUsers({ page_number: 1, page_size: 999 })
      initializeStateFromQuery(route.query)
      await refreshSpendings()
      firstLoad.value = false
    })

    const initializeStateFromQuery = (query: any) => {
      pagination.value.currentPage = parseInt(query.page) || 1
      pagination.value.pageSize = parseInt(query.pageSize) || 50
      filters.value.userId = query.userId || ''
      filters.value.fromDate = query.fromDate || ''
      filters.value.toDate = query.toDate || ''
      filters.value.color = query.color || ''
    }

    const hasActiveFilters = computed(() => {
      return !!(filters.value.userId || filters.value.fromDate || filters.value.toDate || filters.value.color)
    })

    const refreshSpendings = async () => {
      try {
        const queryParams: any = {
          page: pagination.value.currentPage,
          pageSize: pagination.value.pageSize,
        }

        // Add filters to query params if they exist
        if (filters.value.userId) queryParams.userId = filters.value.userId
        if (filters.value.fromDate) queryParams.fromDate = filters.value.fromDate
        if (filters.value.toDate) queryParams.toDate = filters.value.toDate
        if (filters.value.color) queryParams.color = filters.value.color

        router.push({ query: queryParams })

        const params: any = {
          page_number: pagination.value.currentPage,
          page_size: pagination.value.pageSize,
        }

        // Add filters to API params
        if (filters.value.userId) params.application_user_id = filters.value.userId
        if (filters.value.fromDate) params.from_date = filters.value.fromDate
        if (filters.value.toDate) params.to_date = filters.value.toDate
        if (filters.value.color) params.color = filters.value.color

        const data = await getSpendings(params)
        if (data) {
          pagination.value = {
            currentPage: data.currentPage,
            pageSize: pagination.value.pageSize,
            totalCount: data.totalCount,
            totalPages: data.totalPages,
          }
        }
      } catch (error) {
        console.error('Error fetching spendings:', error)
      }
    }

    const applyFilters = async () => {
      pagination.value.currentPage = 1
      await refreshSpendings()
    }

    const clearAllFilters = async () => {
      filters.value = {
        userId: '',
        fromDate: '',
        toDate: '',
        color: ''
      }
      pagination.value.currentPage = 1
      await refreshSpendings()
    }

    const changePage = async (pageNumber: number) => {
      pagination.value.currentPage += pageNumber
      await refreshSpendings()
    }

    const formatCurrency = (amount: number) => {
      return new Intl.NumberFormat('hu-HU', {
        style: 'currency',
        currency: 'HUF'
      }).format(amount)
    }

    const formatDate = (dateString: string) => {
      const date = new Date(dateString)
      return date.toLocaleDateString('hu-HU', {
        year: 'numeric',
        month: 'long',
        day: 'numeric'
      })
    }

    const getPaymentMethodIcon = (method: PaymentMethod) => {
      return method === PaymentMethod.Card ? 'fas fa-credit-card' : 'fas fa-money-bill-wave'
    }

    const getPaymentMethodText = (method: PaymentMethod) => {
      return method === PaymentMethod.Card ? t('card') : t('cash')
    }

    /* MODAL ACTIONS */

    const SpendingCreateModalRef = ref<InstanceType<typeof SpendingCreateModal> | null>(null)
    const openCreateModal = () => {
      if (SpendingCreateModalRef.value) {
        SpendingCreateModalRef.value.open()
      }
    }

    const SpendingEditModalRef = ref<InstanceType<typeof SpendingEditModal> | null>(null)
    const openEditModal = (spending: SpendingItemResponse) => {
      if (SpendingEditModalRef.value) {
        SpendingEditModalRef.value.open(spending)
      }
    }

    const SpendingDeleteModalRef = ref<InstanceType<typeof SpendingDeleteModal> | null>(null)
    const openDeleteModal = (id: string) => {
      if (SpendingDeleteModalRef.value) {
        SpendingDeleteModalRef.value.open([id])
      }
    }

    const handleDelete = async () => {
      await refreshSpendings()
      if (pagination.value.currentPage > 1 && spendings.value.length === 0) {
        changePage(-1)
      }
    }

    return {
      spendings,
      users,
      filters,
      pagination,
      hasActiveFilters,
      refreshSpendings,
      applyFilters,
      clearAllFilters,
      changePage,
      formatCurrency,
      formatDate,
      getPaymentMethodIcon,
      getPaymentMethodText,
      SpendingCreateModalRef,
      openCreateModal,
      SpendingEditModalRef,
      openEditModal,
      SpendingDeleteModalRef,
      openDeleteModal,
      handleDelete,
    }
  },
})
</script>
