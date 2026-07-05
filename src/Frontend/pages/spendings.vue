<template>
  <div class="rounded-lg bg-white dark:bg-gray-800 shadow-xl dark:text-white px-4 py-4 md:px-8 md:py-6">
    <!-- Top Bar -->
    <div class="flex items-center justify-between gap-4 mb-6">
      <button class="btn btn-md bg-primary text-white capitalize-block px-6 py-2 hover:bg-primary-dark transition-colors" @click="openCreateModal">
        {{ $t('create') }}
      </button>

      <div class="flex items-center gap-2">
        <!-- Month Navigation -->
        <button
          @click="navigateMonth(-1)"
          class="p-2 bg-white dark:bg-gray-700 border border-gray-300 dark:border-gray-600 rounded-md text-gray-700 dark:text-gray-300 hover:bg-gray-50 dark:hover:bg-gray-600 transition-colors"
          :title="$t('previous_month')"
        >
          <i class="fas fa-chevron-left"></i>
        </button>

        <div class="px-4 py-2 bg-primary text-white rounded-md text-sm font-semibold min-w-[140px] text-center">
          {{ currentMonthLabel }}
        </div>

        <button
          @click="navigateMonth(1)"
          class="p-2 bg-white dark:bg-gray-700 border border-gray-300 dark:border-gray-600 rounded-md text-gray-700 dark:text-gray-300 hover:bg-gray-50 dark:hover:bg-gray-600 transition-colors"
          :title="$t('next_month')"
        >
          <i class="fas fa-chevron-right"></i>
        </button>

        <button
          v-if="!isCurrentMonth"
          @click="resetToCurrentMonth"
          class="px-3 py-2 bg-gray-200 dark:bg-gray-700 text-gray-700 dark:text-gray-300 rounded-md text-sm hover:bg-gray-300 dark:hover:bg-gray-600 transition-colors"
        >
          <i class="fas fa-calendar-day mr-1"></i>
          {{ $t('current_month') }}
        </button>
      </div>
    </div>

    <!-- User Cards Grid -->
    <div v-if="Object.keys(spendingsByUser).length > 0" class="grid grid-cols-1 lg:grid-cols-2 xl:grid-cols-2 gap-4">
      <div
        v-for="(userSpendings, userId) in spendingsByUser"
        :key="userId"
        class="bg-gradient-to-br from-white to-gray-50 dark:from-gray-700 dark:to-gray-750 rounded-xl border border-gray-200 dark:border-gray-600 shadow-sm overflow-hidden"
      >
        <!-- User Header -->
        <div class="bg-primary to-primary-dark p-4">
          <div class="flex items-center gap-3 mb-3">
            <div class="w-10 h-10 rounded-full bg-white/20 backdrop-blur-sm flex items-center justify-center text-white font-bold text-sm">
              {{ getUserInitials(userId) }}
            </div>
            <div class="flex-1">
              <h3 class="text-white font-semibold text-base">{{ getUserName(userId) }}</h3>
              <p class="text-white/80 text-xs">{{ userSpendings.length }} {{ $t('spendings') }}</p>
            </div>
          </div>

          <!-- Financial Summary -->
          <div class="grid grid-cols-3 gap-2 pt-3 border-t border-white/20">
            <div class="text-center">
              <p class="text-white/60 text-xs mb-1">{{ $t('income') }}</p>
              <p class="text-white font-bold text-sm">{{ formatCurrency(getUserIncome(userId)) }}</p>
            </div>
            <div class="text-center">
              <p class="text-white/60 text-xs mb-1">{{ $t('spent') }}</p>
              <p class="text-white font-bold text-sm">{{ formatCurrency(getUserTotal(userSpendings)) }}</p>
            </div>
            <div class="text-center">
              <p class="text-white/60 text-xs mb-1">{{ $t('remaining') }}</p>
              <p class="text-white font-bold text-sm" :class="getUserRemaining(userId, userSpendings) < 0 ? 'text-red-300' : 'text-green-300'">
                {{ formatCurrency(getUserRemaining(userId, userSpendings)) }}
              </p>
            </div>
          </div>
        </div>

        <!-- Spendings Table -->
        <div class="max-h-96 overflow-y-auto">
          <table class="w-full text-xs">
            <thead class="bg-gray-100 dark:bg-gray-700 sticky top-0">
              <tr class="border-b border-gray-200 dark:border-gray-600">
                <th class="px-2 py-2 text-left font-semibold text-gray-700 dark:text-gray-300">
                  <button @click="toggleSort('item')" class="flex items-center gap-1 hover:text-primary transition-colors">
                    {{ $t('item') }}
                    <i v-if="sortBy === 'item'" :class="sortDirection === 'asc' ? 'fas fa-sort-up' : 'fas fa-sort-down'" class="text-xs"></i>
                    <i v-else class="fas fa-sort text-xs opacity-30"></i>
                  </button>
                </th>
                <th class="px-2 py-2 text-left font-semibold text-gray-700 dark:text-gray-300">
                  <button @click="toggleSort('date')" class="flex items-center gap-1 hover:text-primary transition-colors">
                    {{ $t('date') }}
                    <i v-if="sortBy === 'date'" :class="sortDirection === 'asc' ? 'fas fa-sort-up' : 'fas fa-sort-down'" class="text-xs"></i>
                    <i v-else class="fas fa-sort text-xs opacity-30"></i>
                  </button>
                </th>
                <th class="px-2 py-2 text-center font-semibold text-gray-700 dark:text-gray-300">
                  <button @click="toggleSort('payment')" class="flex items-center gap-1 hover:text-primary transition-colors mx-auto">
                    {{ $t('payment') }}
                    <i v-if="sortBy === 'payment'" :class="sortDirection === 'asc' ? 'fas fa-sort-up' : 'fas fa-sort-down'" class="text-xs"></i>
                    <i v-else class="fas fa-sort text-xs opacity-30"></i>
                  </button>
                </th>
                <th class="px-2 py-2 text-right font-semibold text-gray-700 dark:text-gray-300">
                  <button @click="toggleSort('amount')" class="flex items-center gap-1 hover:text-primary transition-colors ml-auto">
                    {{ $t('amount') }}
                    <i v-if="sortBy === 'amount'" :class="sortDirection === 'asc' ? 'fas fa-sort-up' : 'fas fa-sort-down'" class="text-xs"></i>
                    <i v-else class="fas fa-sort text-xs opacity-30"></i>
                  </button>
                </th>
                <th class="px-2 py-2 text-center font-semibold text-gray-700 dark:text-gray-300">{{ $t('actions') }}</th>
              </tr>
            </thead>
            <tbody>
              <tr
                v-for="spending in userSpendings"
                :key="spending.id"
                class="border-b border-gray-100 dark:border-gray-700 hover:bg-gray-50 dark:hover:bg-gray-700/50 transition-colors cursor-pointer"
                @click="openEditModal(spending)"
              >
                <td class="px-2 py-2">
                  <div class="flex items-center gap-2">
                    <div
                      v-if="spending.color"
                      class="w-3 h-3 rounded-sm flex-shrink-0"
                      :style="{ backgroundColor: spending.color }"
                    ></div>
                    <div class="flex-1 min-w-0">
                      <p class="font-medium text-gray-900 dark:text-white truncate">{{ spending.item }}</p>
                      <p v-if="spending.comment" class="text-gray-500 dark:text-gray-400 truncate text-xs">{{ spending.comment }}</p>
                    </div>
                  </div>
                </td>
                <td class="px-2 py-2 text-gray-600 dark:text-gray-400 whitespace-nowrap">
                  {{ formatDateShort(spending.date) }}
                </td>
                <td class="px-2 py-2 text-center">
                  <i
                    :class="getPaymentMethodIcon(spending.paymentMethod)"
                    class="text-gray-600 dark:text-gray-400"
                    :title="getPaymentMethodText(spending.paymentMethod)"
                  ></i>
                </td>
                <td class="px-2 py-2 text-right font-semibold text-gray-900 dark:text-white whitespace-nowrap">
                  {{ formatCurrency(spending.amount) }}
                </td>
                <td class="px-2 py-2">
                  <div class="flex items-center justify-center">
                    <button
                      @click.stop="openDeleteModal(spending.id)"
                      class="p-1 text-red-600 hover:bg-red-50 dark:hover:bg-red-900/20 rounded transition-colors"
                      :title="$t('delete')"
                    >
                      <i class="fas fa-trash text-xs"></i>
                    </button>
                  </div>
                </td>
              </tr>
            </tbody>
          </table>
        </div>
      </div>
    </div>

    <!-- Empty State -->
    <div v-else class="text-center py-16">
      <div class="w-24 h-24 mx-auto mb-4 rounded-full bg-gray-100 dark:bg-gray-700 flex items-center justify-center">
        <i class="fas fa-wallet text-4xl text-gray-400"></i>
      </div>
      <h3 class="text-lg font-medium text-gray-900 dark:text-white mb-2">{{ $t('no_spendings_found') }}</h3>
      <p class="text-sm text-gray-500 dark:text-gray-400">{{ $t('create_your_first_spending') }}</p>
    </div>

    <!-- Overall Statistics -->
    <div v-if="Object.keys(spendingsByUser).length > 0" class="mt-6 bg-gradient-to-r from-gray-50 to-white dark:from-gray-700 dark:to-gray-750 rounded-lg border border-gray-200 dark:border-gray-600 p-6">
      <div class="flex items-center justify-between mb-4">
        <div class="flex items-center gap-2">
          <div class="w-8 h-8 rounded-lg bg-primary/10 flex items-center justify-center">
            <i class="fas fa-chart-line text-primary text-sm"></i>
          </div>
          <span class="text-sm text-gray-700 dark:text-gray-300">
            <span class="font-semibold">{{ totalSpendingsCount }}</span> {{ $t('total_spendings') }}
          </span>
        </div>
      </div>

      <!-- Financial Summary Grid -->
      <div class="grid grid-cols-3 gap-6">
        <div class="text-center p-4 bg-green-50 dark:bg-green-900/20 rounded-lg border border-green-200 dark:border-green-800">
          <p class="text-sm text-gray-600 dark:text-gray-400 mb-1">{{ $t('grand_income') }}</p>
          <p class="text-2xl font-bold text-green-600 dark:text-green-400">{{ formatCurrency(grandIncome) }}</p>
        </div>
        <div class="text-center p-4 bg-red-50 dark:bg-red-900/20 rounded-lg border border-red-200 dark:border-red-800">
          <p class="text-sm text-gray-600 dark:text-gray-400 mb-1">{{ $t('grand_spending') }}</p>
          <p class="text-2xl font-bold text-red-600 dark:text-red-400">{{ formatCurrency(grandTotal) }}</p>
        </div>
        <div class="text-center p-4 bg-primary/10 rounded-lg border border-primary/20">
          <p class="text-sm text-gray-600 dark:text-gray-400 mb-1">{{ $t('grand_remaining') }}</p>
          <p class="text-2xl font-bold" :class="grandRemaining < 0 ? 'text-red-600 dark:text-red-400' : 'text-green-600 dark:text-green-400'">
            {{ formatCurrency(grandRemaining) }}
          </p>
        </div>
      </div>
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
import { useIncomeStore } from '~/stores/IncomeStore'
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
    const { incomes } = storeToRefs(useIncomeStore())
    const router = useRouter()
    const route = useRoute()

    // Track the viewing month separately
    const now = new Date()
    const viewingYear = ref(now.getFullYear())
    const viewingMonth = ref(now.getMonth()) // 0-11

    // Helper to format date for input
    const formatDate = (date: Date) => {
      const year = date.getFullYear()
      const month = String(date.getMonth() + 1).padStart(2, '0')
      const day = String(date.getDate()).padStart(2, '0')
      return `${year}-${month}-${day}`
    }

    // Get first and last day of a specific month
    const getMonthDates = (year: number, month: number) => {
      const firstDay = new Date(year, month, 1)
      const lastDay = new Date(year, month + 1, 0)
      return {
        fromDate: formatDate(firstDay),
        toDate: formatDate(lastDay)
      }
    }

    // Initialize filters with current month
    const initialDates = getMonthDates(viewingYear.value, viewingMonth.value)
    const filters = ref({
      fromDate: initialDates.fromDate,
      toDate: initialDates.toDate,
    })
    const sortBy = ref<'item' | 'date' | 'payment' | 'amount'>('date')
    const sortDirection = ref<'asc' | 'desc'>('desc')
    const firstLoad = ref(true)

    onMounted(async () => {
      setActivePageTitle('Spendings')
      // Load users first
      await getUsers({ page_number: 1, page_size: 999 })
      // Load incomes for all users
      await loadAllIncomes()
      initializeStateFromQuery(route.query)
      await refreshSpendings()
      firstLoad.value = false
    })

    const loadAllIncomes = async () => {
      const { getIncomesByUserId } = useIncomeStore()
      // Load incomes for each user
      for (const user of users.value) {
        await getIncomesByUserId(user.id)
      }
    }

    const initializeStateFromQuery = (query: any) => {
      // If dates in query, parse them to set viewing month
      if (query.fromDate && query.toDate) {
        filters.value.fromDate = query.fromDate
        filters.value.toDate = query.toDate

        // Extract year and month from fromDate
        const [year, month] = query.fromDate.split('-').map(Number)
        viewingYear.value = year
        viewingMonth.value = month - 1 // Convert to 0-indexed
      }
      sortBy.value = query.sortBy || 'date'
      sortDirection.value = query.sortDirection || 'desc'
    }

    // Month navigation
    const currentMonthLabel = computed(() => {
      const date = new Date(viewingYear.value, viewingMonth.value, 1)
      return date.toLocaleDateString('hu-HU', { month: 'long', year: 'numeric' })
    })

    const isIncomeInViewMonth = (incomeDate: string) => {
      if (!filters.value.fromDate || !filters.value.toDate) {
        return true
      }

      return incomeDate >= filters.value.fromDate && incomeDate <= filters.value.toDate
    }

    const isCurrentMonth = computed(() => {
      const today = new Date()
      return viewingYear.value === today.getFullYear() && viewingMonth.value === today.getMonth()
    })

    const navigateMonth = (direction: number) => {
      // Update viewing month
      let newMonth = viewingMonth.value + direction
      let newYear = viewingYear.value

      // Handle month overflow/underflow
      if (newMonth > 11) {
        newMonth = 0
        newYear++
      } else if (newMonth < 0) {
        newMonth = 11
        newYear--
      }

      viewingYear.value = newYear
      viewingMonth.value = newMonth

      // Update filters
      const dates = getMonthDates(newYear, newMonth)
      filters.value.fromDate = dates.fromDate
      filters.value.toDate = dates.toDate

      refreshSpendings()
    }

    const resetToCurrentMonth = () => {
      const today = new Date()
      viewingYear.value = today.getFullYear()
      viewingMonth.value = today.getMonth()

      const dates = getMonthDates(viewingYear.value, viewingMonth.value)
      filters.value.fromDate = dates.fromDate
      filters.value.toDate = dates.toDate

      refreshSpendings()
    }

    const toggleSort = (column: 'item' | 'date' | 'payment' | 'amount') => {
      if (sortBy.value === column) {
        // Toggle direction if clicking same column
        sortDirection.value = sortDirection.value === 'asc' ? 'desc' : 'asc'
      } else {
        // New column, default to ascending
        sortBy.value = column
        sortDirection.value = 'asc'
      }
      updateQueryParams()
    }

    const updateQueryParams = () => {
      const queryParams: any = {}

      if (filters.value.fromDate) queryParams.fromDate = filters.value.fromDate
      if (filters.value.toDate) queryParams.toDate = filters.value.toDate
      queryParams.sortBy = sortBy.value
      queryParams.sortDirection = sortDirection.value

      router.push({ query: queryParams })
    }


    // Group spendings by user
    const spendingsByUser = computed(() => {
      const grouped: Record<string, SpendingItemResponse[]> = {}

      spendings.value.forEach(spending => {
        const userId = spending.applicationUserId
        if (!grouped[userId]) {
          grouped[userId] = []
        }
        grouped[userId].push(spending)
      })

      // Sort spendings within each user based on current sort settings
      Object.keys(grouped).forEach(userId => {
        grouped[userId].sort((a, b) => {
          let comparison = 0

          switch (sortBy.value) {
            case 'item':
              comparison = a.item.localeCompare(b.item)
              break
            case 'date':
              comparison = new Date(a.date).getTime() - new Date(b.date).getTime()
              break
            case 'payment':
              comparison = a.paymentMethod - b.paymentMethod
              break
            case 'amount':
              comparison = a.amount - b.amount
              break
          }

          return sortDirection.value === 'asc' ? comparison : -comparison
        })
      })

      return grouped
    })

    const totalSpendingsCount = computed(() => {
      return spendings.value.length
    })

    const grandTotal = computed(() => {
      return spendings.value.reduce((sum, spending) => sum + spending.amount, 0)
    })

    const grandIncome = computed(() => {
      return incomes.value
        .filter(income => income.date && isIncomeInViewMonth(income.date))
        .reduce((sum, income) => sum + (income.amount || 0), 0)
    })

    const grandRemaining = computed(() => {
      return grandIncome.value - grandTotal.value
    })

    const getUserName = (userId: string) => {
      const user = users.value.find(u => u.id === userId)
      return user ? `${user.lastName} ${user.firstName}` : t('unknown_user')
    }

    const getUserInitials = (userId: string) => {
      const user = users.value.find(u => u.id === userId)
      if (!user) return '?'
      return `${user.firstName?.charAt(0) || ''}${user.lastName?.charAt(0) || ''}`
    }

    const getUserTotal = (userSpendings: SpendingItemResponse[]) => {
      return userSpendings.reduce((sum, spending) => sum + spending.amount, 0)
    }

    const getUserIncome = (userId: string) => {
      const userIncomes = incomes.value.filter(income =>
        income.applicationUserId === userId && income.date && isIncomeInViewMonth(income.date)
      )
      return userIncomes.reduce((sum, income) => sum + (income.amount || 0), 0)
    }

    const getUserRemaining = (userId: string, userSpendings: SpendingItemResponse[]) => {
      const income = getUserIncome(userId)
      const spent = getUserTotal(userSpendings)
      return income - spent
    }

    const refreshSpendings = async () => {
      try {
        updateQueryParams()

        const params: any = {
          page_number: 1,
          page_size: 9999, // Get all spendings
        }

        // Add filters to API params
        if (filters.value.fromDate) params.fromDate = filters.value.fromDate
        if (filters.value.toDate) params.toDate = filters.value.toDate

        await getSpendings(params)
      } catch (error) {
        console.error('Error fetching spendings:', error)
      }
    }


    const formatCurrency = (amount: number) => {
      return new Intl.NumberFormat('hu-HU', {
        style: 'currency',
        currency: 'HUF',
        minimumFractionDigits: 0,
        maximumFractionDigits: 0
      }).format(amount)
    }

    const formatDateShort = (dateString: string) => {
      const date = new Date(`${dateString}T00:00:00`)
      return date.toLocaleDateString('hu-HU', {
        month: '2-digit',
        day: '2-digit'
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
    const openCreateModal = async () => {
      // Refresh users before opening modal to ensure latest data
      await getUsers({ page_number: 1, page_size: 999 })
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
    }

    return {
      spendings,
      users,
      filters,
      sortBy,
      sortDirection,
      currentMonthLabel,
      isCurrentMonth,
      spendingsByUser,
      totalSpendingsCount,
      grandTotal,
      grandIncome,
      grandRemaining,
      getUserName,
      getUserInitials,
      getUserTotal,
      getUserIncome,
      getUserRemaining,
      toggleSort,
      navigateMonth,
      resetToCurrentMonth,
      refreshSpendings,
      formatCurrency,
      formatDateShort,
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
