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
          <div class="grid grid-cols-2 lg:grid-cols-5 gap-2 pt-3 border-t border-white/20">
            <div class="text-center">
              <p class="text-white/60 text-xs mb-1">{{ $t('income') }}</p>
              <p class="text-white font-bold text-sm">{{ formatCurrency(getUserIncome(userId)) }}</p>
            </div>
            <div class="text-center">
              <p class="text-white/60 text-xs mb-1">{{ $t('card') }} {{ $t('spendings') }}</p>
              <p class="text-white font-bold text-sm">{{ formatCurrency(getUserCardSpendings(userSpendings)) }}</p>
            </div>
            <div class="text-center">
              <p class="text-white/60 text-xs mb-1">{{ $t('cash') }} {{ $t('spendings') }}</p>
              <p class="text-white font-bold text-sm">{{ formatCurrency(getUserCashSpendings(userSpendings)) }}</p>
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

          <div class="mt-4 flex flex-wrap items-center justify-between gap-3 rounded-lg bg-white/10 px-3 py-2">
            <div class="flex items-center gap-2">
              <label :for="`pageSize-${userId}`" class="text-xs font-medium !text-white" style="color: #fff;">
                {{ $t('items_per_page') }}
              </label>
              <select
                :id="`pageSize-${userId}`"
                v-model.number="userPagination[userId].pageSize"
                class="w-24 px-2 py-1 rounded-md border border-white/20 bg-white/90 text-sm text-gray-900 focus:outline-none focus:ring-2 focus:ring-primary cursor-pointer"
                @change="changeUserPageSize(userId)"
              >
                <option v-for="size in pageSizeOptions" :key="size" :value="size">
                  {{ size }}
                </option>
              </select>
            </div>

            <div class="flex items-center gap-2" v-if="getUserTotalPages(userSpendings, userPagination[userId].pageSize) > 1">
              <button
                type="button"
                class="w-9 h-9 rounded-md border border-white/20 bg-white/90 text-gray-700 hover:bg-white transition-colors disabled:opacity-40 disabled:cursor-not-allowed"
                :disabled="userPagination[userId].currentPage === 1"
                @click="changeUserPage(userId, -1, userSpendings)"
                :title="$t('previous')"
              >
                <i class="fas fa-chevron-left text-xs"></i>
              </button>

              <div class="min-w-[96px] px-3 py-1.5 rounded-md bg-white/15 text-white text-center">
                <div class="text-[10px] uppercase opacity-80">{{ $t('page') }}</div>
                <div class="text-sm font-semibold">
                  {{ userPagination[userId].currentPage }} / {{ getUserTotalPages(userSpendings, userPagination[userId].pageSize) }}
                </div>
              </div>

              <button
                type="button"
                class="w-9 h-9 rounded-md border border-white/20 bg-white/90 text-gray-700 hover:bg-white transition-colors disabled:opacity-40 disabled:cursor-not-allowed"
                :disabled="userPagination[userId].currentPage === getUserTotalPages(userSpendings, userPagination[userId].pageSize)"
                @click="changeUserPage(userId, 1, userSpendings)"
                :title="$t('next')"
              >
                <i class="fas fa-chevron-right text-xs"></i>
              </button>
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
                  <button @click="toggleSort('paymentMethod')" class="flex items-center gap-1 hover:text-primary transition-colors mx-auto">
                    {{ $t('payment') }}
                    <i v-if="sortBy === 'paymentMethod'" :class="sortDirection === 'asc' ? 'fas fa-sort-up' : 'fas fa-sort-down'" class="text-xs"></i>
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
                v-for="spending in getPagedUserSpendings(userId, userSpendings)"
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
    <div v-if="totalSpendingsCount > 0" class="mt-6 bg-gradient-to-r from-gray-50 to-white dark:from-gray-700 dark:to-gray-750 rounded-lg border border-gray-200 dark:border-gray-600 p-6">
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
      <div class="grid grid-cols-1 md:grid-cols-2 xl:grid-cols-4 gap-4">
        <div class="text-center p-4 bg-green-50 dark:bg-green-900/20 rounded-lg border border-green-200 dark:border-green-800">
          <p class="text-sm text-gray-600 dark:text-gray-400 mb-1">{{ $t('grand_income') }}</p>
          <p class="text-2xl font-bold text-green-600 dark:text-green-400">{{ formatCurrency(grandIncome) }}</p>
        </div>
        <div class="text-center p-4 bg-blue-50 dark:bg-blue-900/20 rounded-lg border border-blue-200 dark:border-blue-800">
          <p class="text-sm text-gray-600 dark:text-gray-400 mb-1">{{ $t('card') }} {{ $t('spendings') }}</p>
          <p class="text-2xl font-bold text-blue-600 dark:text-blue-400">{{ formatCurrency(cardSpendingsTotal) }}</p>
        </div>
        <div class="text-center p-4 bg-amber-50 dark:bg-amber-900/20 rounded-lg border border-amber-200 dark:border-amber-800">
          <p class="text-sm text-gray-600 dark:text-gray-400 mb-1">{{ $t('cash') }} {{ $t('spendings') }}</p>
          <p class="text-2xl font-bold text-amber-600 dark:text-amber-400">{{ formatCurrency(cashSpendingsTotal) }}</p>
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
import { ALL_RECORDS_PAGE_SIZE } from '~/core/pagination'
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
    const { t, locale } = useI18n()
    const { setActivePageTitle } = useGlobalStore()
    const spendingStore = useSpendingStore()
    const { getAllSpendings } = spendingStore
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
    const pageSizeOptions = [10, 50, 100]
    const sortBy = ref<'createdAt' | 'item' | 'date' | 'paymentMethod' | 'amount'>('createdAt')
    const sortDirection = ref<'asc' | 'desc'>('desc')
    const userPagination = reactive<Record<string, { currentPage: number; pageSize: number }>>({})
    const allSpendings = ref<SpendingItemResponse[]>([])

    onMounted(async () => {
      setActivePageTitle('spendings')
      // Load users first
      await getUsers({ page_number: 1, page_size: ALL_RECORDS_PAGE_SIZE })
      // Load incomes for all users
      await loadAllIncomes()
      initializeStateFromQuery(route.query)
      await refreshSpendings()
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
      sortBy.value = query.sortBy || 'createdAt'
      sortDirection.value = query.sortDirection || 'desc'
    }

    // Month navigation
    const currentMonthLabel = computed(() => {
      const date = new Date(viewingYear.value, viewingMonth.value, 1)
      const resolvedLocale = locale.value === 'hu' ? 'hu-HU' : 'en-US'
      return date.toLocaleDateString(resolvedLocale, { month: 'long', year: 'numeric' })
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
      resetAllUserPagination()
      refreshSpendings()
    }

    const resetToCurrentMonth = () => {
      const today = new Date()
      viewingYear.value = today.getFullYear()
      viewingMonth.value = today.getMonth()

      const dates = getMonthDates(viewingYear.value, viewingMonth.value)
      filters.value.fromDate = dates.fromDate
      filters.value.toDate = dates.toDate
      resetAllUserPagination()
      refreshSpendings()
    }

    const toggleSort = (column: 'item' | 'date' | 'paymentMethod' | 'amount' | 'createdAt') => {
      if (sortBy.value === column) {
        // Toggle direction if clicking same column
        sortDirection.value = sortDirection.value === 'asc' ? 'desc' : 'asc'
      } else {
        // New column, default to ascending
        sortBy.value = column
        sortDirection.value = 'asc'
      }
      resetAllUserPagination()
      refreshSpendings()
    }

    const updateQueryParams = async () => {
      const queryParams: any = {}

      if (filters.value.fromDate) queryParams.fromDate = filters.value.fromDate
      if (filters.value.toDate) queryParams.toDate = filters.value.toDate
      queryParams.sortBy = sortBy.value
      queryParams.sortDirection = sortDirection.value

      try {
        await router.replace({ query: queryParams })
      } catch {
        // Ignore duplicate navigation when the query already matches.
      }
    }


    // Group spendings by user
    const spendingsByUser = computed(() => {
      const grouped: Record<string, SpendingItemResponse[]> = {}

      allSpendings.value.forEach(spending => {
        const userId = spending.applicationUserId
        if (!grouped[userId]) {
          grouped[userId] = []
        }
        grouped[userId].push(spending)
      })

      Object.keys(grouped).forEach(userId => {
        ensureUserPagination(userId)
      })

      return grouped
    })

    const ensureUserPagination = (userId: string) => {
      if (!userPagination[userId]) {
        userPagination[userId] = {
          currentPage: 1,
          pageSize: 50,
        }
      }

      return userPagination[userId]
    }

    const resetAllUserPagination = () => {
      Object.values(userPagination).forEach(pagination => {
        pagination.currentPage = 1
      })
    }

    const getUserTotalPages = (userSpendings: SpendingItemResponse[], pageSize: number) => {
      return Math.max(1, Math.ceil(userSpendings.length / pageSize))
    }

    const getPagedUserSpendings = (userId: string, userSpendings: SpendingItemResponse[]) => {
      const pagination = ensureUserPagination(userId)
      const totalPages = getUserTotalPages(userSpendings, pagination.pageSize)
      if (pagination.currentPage > totalPages) {
        pagination.currentPage = totalPages
      }
      const startIndex = (pagination.currentPage - 1) * pagination.pageSize
      return userSpendings.slice(startIndex, startIndex + pagination.pageSize)
    }

    const totalSpendingsCount = computed(() => {
      return allSpendings.value.length
    })

    const grandTotal = computed(() => {
      return allSpendings.value.reduce((sum, spending) => sum + spending.amount, 0)
    })

    const cardSpendingsTotal = computed(() => {
      return allSpendings.value
        .filter(spending => spending.paymentMethod === PaymentMethod.Card)
        .reduce((sum, spending) => sum + spending.amount, 0)
    })

    const cashSpendingsTotal = computed(() => {
      return allSpendings.value
        .filter(spending => spending.paymentMethod === PaymentMethod.Cash)
        .reduce((sum, spending) => sum + spending.amount, 0)
    })

    const grandIncome = computed(() => {
      return incomes.value
        .filter(income => income.date && isIncomeInViewMonth(income.date))
        .reduce((sum, income) => sum + (income.amount || 0), 0)
    })

    const grandRemaining = computed(() => {
      return grandIncome.value - grandTotal.value
    })

    const changeUserPage = (userId: string, direction: number, userSpendings: SpendingItemResponse[]) => {
      const pagination = ensureUserPagination(userId)
      const totalPages = getUserTotalPages(userSpendings, pagination.pageSize)
      const nextPage = pagination.currentPage + direction

      if (nextPage < 1 || nextPage > totalPages) {
        return
      }

      pagination.currentPage = nextPage
    }

    const changeUserPageSize = (userId: string) => {
      const pagination = ensureUserPagination(userId)
      pagination.currentPage = 1
    }

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

    const getUserCardSpendings = (userSpendings: SpendingItemResponse[]) => {
      return userSpendings
        .filter(spending => spending.paymentMethod === PaymentMethod.Card)
        .reduce((sum, spending) => sum + spending.amount, 0)
    }

    const getUserCashSpendings = (userSpendings: SpendingItemResponse[]) => {
      return userSpendings
        .filter(spending => spending.paymentMethod === PaymentMethod.Cash)
        .reduce((sum, spending) => sum + spending.amount, 0)
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
        await updateQueryParams()

        const params: any = {
          pageNumber: 1,
          pageSize: ALL_RECORDS_PAGE_SIZE,
          sortBy: sortBy.value,
          sortDescending: sortDirection.value === 'desc',
        }

        // Add filters to API params
        if (filters.value.fromDate) params.fromDate = filters.value.fromDate
        if (filters.value.toDate) params.toDate = filters.value.toDate

        const allResult = await getAllSpendings(params)
        allSpendings.value = allResult
        Object.keys(spendingsByUser.value).forEach(userId => {
          ensureUserPagination(userId)
        })
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
      const resolvedLocale = locale.value === 'hu' ? 'hu-HU' : 'en-US'
      return date.toLocaleDateString(resolvedLocale, {
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
      await getUsers({ page_number: 1, page_size: ALL_RECORDS_PAGE_SIZE })
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
      users,
      filters,
      pageSizeOptions,
      userPagination,
      sortBy,
      sortDirection,
      currentMonthLabel,
      isCurrentMonth,
      spendingsByUser,
      totalSpendingsCount,
      grandTotal,
      cardSpendingsTotal,
      cashSpendingsTotal,
      grandIncome,
      grandRemaining,
      getUserName,
      getUserInitials,
      getUserTotal,
      getUserIncome,
      getUserCardSpendings,
      getUserCashSpendings,
      getUserRemaining,
      getUserTotalPages,
      getPagedUserSpendings,
      changeUserPage,
      changeUserPageSize,
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
