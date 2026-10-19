<template>
  <div class="space-y-5 md:space-y-7">
    <section class="flex flex-col justify-between gap-4 sm:flex-row sm:items-end">
      <div>
        <p class="mb-2 text-xs font-semibold uppercase tracking-[.18em] text-[#718073]">{{ currentMonthLabel }} · {{ $t('overview') }}</p>
        <h2 class="font-display text-3xl font-semibold tracking-tight text-[#253d32] md:text-4xl">{{ $t('hello') }}, {{ displayName }}</h2>
        <p class="mt-2 text-sm text-[#718073]">{{ $t('dashboard_subtitle') }}</p>
      </div>
      <div class="flex items-center gap-2 self-start sm:self-auto">
        <button type="button" class="dashboard-icon-button" :disabled="loading" :title="$t('previous_month')" @click="changeMonth(-1)"><i class="fas fa-chevron-left" /></button>
        <button type="button" class="dashboard-icon-button" :disabled="loading" :title="$t('next_month')" @click="changeMonth(1)"><i class="fas fa-chevron-right" /></button>
        <NuxtLink to="/spendings" class="dashboard-action"><i class="fas fa-arrow-up-right-from-square mr-2 text-xs" />{{ $t('spendings') }}</NuxtLink>
      </div>
    </section>

    <section class="grid grid-cols-1 gap-3 sm:grid-cols-2 xl:grid-cols-4">
      <article class="dashboard-stat">
        <div class="flex items-start justify-between"><span class="dashboard-stat-label">{{ $t('income') }}</span><span class="dashboard-stat-icon text-[#a9c49e]"><i class="fas fa-arrow-down" /></span></div>
        <p class="dashboard-stat-value">{{ formatCurrency(totalIncome) }}</p>
        <p class="dashboard-stat-foot">{{ incomesInMonth.length }} {{ $t('income').toLowerCase() }}</p>
      </article>
      <article class="dashboard-stat">
        <div class="flex items-start justify-between"><span class="dashboard-stat-label">{{ $t('total_spent') }}</span><span class="dashboard-stat-icon text-[#cf8b76]"><i class="fas fa-arrow-up" /></span></div>
        <p class="dashboard-stat-value">{{ formatCurrency(totalSpending) }}</p>
        <p class="dashboard-stat-foot">{{ spendingsInMonth.length }} {{ $t('spendings').toLowerCase() }}</p>
      </article>
      <article class="dashboard-stat" :class="netBalance < 0 ? 'border-[#744c43]/50' : 'border-[#526c51]/50'">
        <div class="flex items-start justify-between"><span class="dashboard-stat-label">{{ $t('net_balance') }}</span><span class="dashboard-stat-icon" :class="netBalance < 0 ? 'text-[#d18a76]' : 'text-[#a9c49e]'"><i class="fas fa-scale-balanced" /></span></div>
        <p class="dashboard-stat-value" :class="netBalance < 0 ? 'text-[#d18a76]' : 'text-[#b6caa8]'">{{ formatCurrency(netBalance) }}</p>
        <p class="dashboard-stat-foot">{{ $t('income') }} − {{ $t('spendings') }}</p>
      </article>
      <article class="dashboard-stat">
        <div class="flex items-start justify-between"><span class="dashboard-stat-label">{{ $t('users') }}</span><span class="dashboard-stat-icon text-[#b9b49a]"><i class="fas fa-users" /></span></div>
        <p class="dashboard-stat-value">{{ users.length }}</p>
        <p class="dashboard-stat-foot">{{ $t('registered_users') }}</p>
      </article>
    </section>

    <section class="grid grid-cols-1 gap-5 xl:grid-cols-[minmax(0,1.75fr)_minmax(300px,.85fr)]">
      <article class="dashboard-panel min-w-0 p-4 sm:p-6">
        <div class="mb-5 flex flex-wrap items-start justify-between gap-3">
          <div>
            <h3 class="dashboard-panel-title">{{ $t('monthly_net_flow') }}</h3>
            <p class="mt-1 text-xs text-[#728075]">{{ $t('spending_trend') }} · {{ currentMonthLabel }}</p>
          </div>
          <div class="flex flex-wrap items-center gap-4 text-[11px] text-[#66756a]">
            <span class="flex items-center gap-2"><i class="h-2 w-2 rounded-full bg-[#9bb98f]" />{{ $t('net_balance') }}</span>
            <span class="flex items-center gap-2"><i class="h-2 w-2 rounded-full bg-[#bd7964]" />{{ $t('spending_trend') }}</span>
          </div>
        </div>
        <div class="relative h-64 w-full sm:h-72">
          <svg viewBox="0 0 800 260" preserveAspectRatio="none" class="h-full w-full overflow-visible" role="img" :aria-label="$t('monthly_net_flow')">
            <defs>
              <linearGradient id="balanceFill" x1="0" x2="0" y1="0" y2="1"><stop offset="0%" stop-color="#9bb98f" stop-opacity=".2" /><stop offset="100%" stop-color="#9bb98f" stop-opacity="0" /></linearGradient>
            </defs>
            <g v-for="line in 5" :key="line">
              <line x1="0" :y1="line * 48" x2="800" :y2="line * 48" stroke="#e2e5da" stroke-width="1" />
            </g>
            <path v-if="hasChartData" :d="balanceAreaPath" fill="url(#balanceFill)" />
            <path v-if="hasChartData" :d="balanceLinePath" fill="none" stroke="#9bb98f" stroke-width="3" vector-effect="non-scaling-stroke" stroke-linecap="round" stroke-linejoin="round" />
            <path v-if="hasChartData" :d="spendingLinePath" fill="none" stroke="#bd7964" stroke-width="1.5" stroke-dasharray="4 6" vector-effect="non-scaling-stroke" stroke-linecap="round" stroke-linejoin="round" />
            <circle v-if="hasChartData" :cx="lastChartPoint.x" :cy="lastChartPoint.y" r="5" fill="#b9d0ab" stroke="#17221a" stroke-width="3" vector-effect="non-scaling-stroke" />
          </svg>
          <div v-if="!hasChartData" class="absolute inset-0 flex items-center justify-center text-sm text-[#728075]">{{ $t('no_chart_data') }}</div>
        </div>
        <div class="mt-3 flex justify-between border-t border-[#e3e6dc] pt-3 text-[10px] text-[#829084]">
          <span v-for="tick in chartTicks" :key="tick.label">{{ tick.label }}</span>
        </div>
      </article>

      <article class="dashboard-panel p-4 sm:p-6">
        <div class="mb-6">
          <h3 class="dashboard-panel-title">{{ $t('monthly_breakdown') }}</h3>
          <p class="mt-1 text-xs text-[#728075]">{{ $t('cash_spent') }} / {{ $t('card_spent') }}</p>
        </div>
        <div class="flex items-center justify-center py-2">
          <div class="dashboard-donut" :style="{ background: donutGradient }">
            <div class="dashboard-donut-center"><span>{{ $t('total') }}</span><strong>{{ formatCompactCurrency(totalSpending) }}</strong></div>
          </div>
        </div>
        <div class="mt-6 space-y-4">
          <div class="flex items-center justify-between gap-4">
            <span class="flex items-center gap-2 text-sm text-[#abb2aa]"><i class="h-2.5 w-2.5 rounded-full bg-[#9bb98f]" />{{ $t('cash_spent') }}</span>
            <span class="text-sm font-semibold text-[#263d31]">{{ formatCurrency(cashSpending) }}</span>
          </div>
          <div class="flex items-center justify-between gap-4">
            <span class="flex items-center gap-2 text-sm text-[#abb2aa]"><i class="h-2.5 w-2.5 rounded-full bg-[#bd7964]" />{{ $t('card_spent') }}</span>
            <span class="text-sm font-semibold text-[#263d31]">{{ formatCurrency(cardSpending) }}</span>
          </div>
          <div class="h-px bg-[#e3e6dc]" />
          <p class="text-xs text-[#829084]">{{ $t('monthly_breakdown') }} · {{ currentMonthLabel }}</p>
        </div>
      </article>
    </section>

    <section class="dashboard-panel overflow-hidden">
      <div class="flex items-center justify-between gap-3 border-b border-[#e3e6dc] px-4 py-4 sm:px-6">
        <div><h3 class="dashboard-panel-title">{{ $t('recent_activity') }}</h3><p class="mt-1 text-xs text-[#728075]">{{ currentMonthLabel }}</p></div>
        <NuxtLink to="/spendings" class="text-xs font-semibold text-[#a9c49e] hover:text-white">{{ $t('view_all') }} <i class="fas fa-arrow-right ml-1" /></NuxtLink>
      </div>
      <div v-if="recentSpendings.length" class="divide-y divide-[#e6e7de]">
        <div v-for="spending in recentSpendings" :key="spending.id" class="flex items-center gap-3 px-4 py-3.5 transition-colors hover:bg-gray-50 sm:px-6">
          <span class="flex h-10 w-10 shrink-0 items-center justify-center rounded-xl" :style="{ color: spending.color || '#a9c49e', backgroundColor: `${spending.color || '#a9c49e'}20` }"><i :class="spending.paymentMethod === PaymentMethod.Card ? 'fas fa-credit-card' : 'fas fa-money-bill-wave'" /></span>
          <div class="min-w-0 flex-1"><p class="truncate text-sm font-medium text-[#263d31]">{{ spending.item }}</p><p class="mt-0.5 text-[11px] text-[#7b887c]">{{ userName(spending.applicationUserId) }} · {{ formatDate(spending.date) }}</p></div>
          <span class="whitespace-nowrap text-sm font-semibold text-[#263d31]">−{{ formatCurrency(spending.amount) }}</span>
        </div>
      </div>
      <div v-else class="px-6 py-10 text-center text-sm text-[#728075]">{{ $t('no_transactions') }}</div>
    </section>
  </div>
</template>

<script setup lang="ts">
import { storeToRefs } from 'pinia'
import { ALL_RECORDS_PAGE_SIZE } from '~/core/pagination'
import { PaymentMethod, type SpendingItemResponse } from '~/models/SpendingModel'
import { useAuthStore } from '~/stores/AuthStore'
import { useGlobalStore } from '~/stores/global'
import { useIncomeStore } from '~/stores/IncomeStore'
import { useSpendingStore } from '~/stores/SpendingStore'
import { useUserStore } from '~/stores/UserStore'

definePageMeta({ middleware: 'auth' })

const { t, locale } = useI18n()
const { account } = storeToRefs(useAuthStore())
const { setActivePageTitle } = useGlobalStore()
const userStore = useUserStore()
const incomeStore = useIncomeStore()
const spendingStore = useSpendingStore()
const { users } = storeToRefs(userStore)
const { incomes } = storeToRefs(incomeStore)
const spendingsInMonth = ref<SpendingItemResponse[]>([])
const viewingYear = ref(new Date().getFullYear())
const viewingMonth = ref(new Date().getMonth())
const loading = ref(false)

const startDate = computed(() => `${viewingYear.value}-${String(viewingMonth.value + 1).padStart(2, '0')}-01`)
const daysInMonth = computed(() => new Date(viewingYear.value, viewingMonth.value + 1, 0).getDate())
const endDate = computed(() => `${viewingYear.value}-${String(viewingMonth.value + 1).padStart(2, '0')}-${String(daysInMonth.value).padStart(2, '0')}`)
const currentMonthLabel = computed(() => new Date(viewingYear.value, viewingMonth.value, 1).toLocaleDateString(locale.value === 'hu' ? 'hu-HU' : 'en-US', { month: 'long', year: 'numeric' }))
const displayName = computed(() => {
  const user = users.value.find(item => item.email.toLowerCase() === account.value.email.toLowerCase())
  return user?.nickName || user?.firstName || account.value.email.split('@')[0] || 'there'
})
const incomesInMonth = computed(() => incomes.value.filter(income => income.date >= startDate.value && income.date <= endDate.value))
const totalIncome = computed(() => incomesInMonth.value.reduce((sum, income) => sum + (income.amount || 0), 0))
const totalSpending = computed(() => spendingsInMonth.value.reduce((sum, spending) => sum + spending.amount, 0))
const netBalance = computed(() => totalIncome.value - totalSpending.value)
const cashSpending = computed(() => spendingsInMonth.value.filter(item => item.paymentMethod === PaymentMethod.Cash).reduce((sum, item) => sum + item.amount, 0))
const cardSpending = computed(() => spendingsInMonth.value.filter(item => item.paymentMethod === PaymentMethod.Card).reduce((sum, item) => sum + item.amount, 0))
const recentSpendings = computed(() => [...spendingsInMonth.value].sort((a, b) => b.date.localeCompare(a.date)).slice(0, 6))
const dailyFlow = computed(() => {
  let balance = 0
  return Array.from({ length: daysInMonth.value }, (_, index) => {
    const day = index + 1
    const dayKey = `${startDate.value.slice(0, 8)}${String(day).padStart(2, '0')}`
    const received = incomesInMonth.value.filter(item => item.date === dayKey).reduce((sum, item) => sum + (item.amount || 0), 0)
    const spent = spendingsInMonth.value.filter(item => item.date === dayKey).reduce((sum, item) => sum + item.amount, 0)
    balance += received - spent
    return { day, balance, spent }
  })
})
const plotBounds = computed(() => {
  const values = dailyFlow.value.map(point => point.balance)
  let low = Math.min(0, ...values)
  let high = Math.max(0, ...values)
  if (low === high) high = low + 1
  const padding = (high - low) * .12
  return { low: low - padding, high: high + padding }
})
const chartPoints = computed(() => dailyFlow.value.map((point, index) => ({
  x: dailyFlow.value.length <= 1 ? 400 : 18 + index / (dailyFlow.value.length - 1) * 764,
  y: 232 - (point.balance - plotBounds.value.low) / (plotBounds.value.high - plotBounds.value.low) * 204,
  spendingY: 226 - point.spent / Math.max(1, ...dailyFlow.value.map(item => item.spent)) * 50,
})))
const toPath = (points: Array<{ x: number; y: number }>) => points.map((point, index) => `${index ? 'L' : 'M'} ${point.x.toFixed(1)} ${point.y.toFixed(1)}`).join(' ')
const balanceLinePath = computed(() => toPath(chartPoints.value))
const balanceAreaPath = computed(() => chartPoints.value.length ? `${balanceLinePath.value} L 782 240 L 18 240 Z` : '')
const spendingLinePath = computed(() => toPath(chartPoints.value.map((point, index) => ({ x: point.x, y: dailyFlow.value[index]?.spent ? point.spendingY : 226 }))))
const lastChartPoint = computed(() => chartPoints.value[chartPoints.value.length - 1] || { x: 0, y: 0 })
const hasChartData = computed(() => totalIncome.value > 0 || totalSpending.value > 0)
const chartTicks = computed(() => [1, Math.round(daysInMonth.value * .25), Math.round(daysInMonth.value * .5), Math.round(daysInMonth.value * .75), daysInMonth.value].map(day => ({ label: String(day).padStart(2, '0') })))
const cashPercent = computed(() => totalSpending.value ? cashSpending.value / totalSpending.value * 100 : 0)
const donutGradient = computed(() => totalSpending.value
  ? `conic-gradient(#9bb98f 0% ${cashPercent.value}%, #bd7964 ${cashPercent.value}% 100%)`
  : 'conic-gradient(#e2e5da 0% 100%)')

const formatCurrency = (amount: number) => new Intl.NumberFormat(locale.value === 'hu' ? 'hu-HU' : 'en-US', { style: 'currency', currency: 'HUF', maximumFractionDigits: 0 }).format(amount)
const formatCompactCurrency = (amount: number) => new Intl.NumberFormat(locale.value === 'hu' ? 'hu-HU' : 'en-US', { style: 'currency', currency: 'HUF', notation: 'compact', maximumFractionDigits: 1 }).format(amount)
const formatDate = (value: string) => new Date(`${value}T00:00:00`).toLocaleDateString(locale.value === 'hu' ? 'hu-HU' : 'en-US', { month: 'short', day: 'numeric' })
const userName = (userId: string) => {
  const user = users.value.find(item => item.id === userId)
  return user?.nickName || user?.firstName || t('unknown_user')
}

const loadDashboard = async () => {
  loading.value = true
  try {
    await userStore.getUsers({ page_number: 1, page_size: ALL_RECORDS_PAGE_SIZE })
    await Promise.all(users.value.map(user => incomeStore.getIncomesByUserId(user.id)))
    spendingsInMonth.value = await spendingStore.getAllSpendings({
      pageNumber: 1,
      pageSize: ALL_RECORDS_PAGE_SIZE,
      fromDate: startDate.value,
      toDate: endDate.value,
      sortBy: 'date',
      sortDescending: true,
    })
  } finally {
    loading.value = false
  }
}

const changeMonth = async (offset: number) => {
  const next = new Date(viewingYear.value, viewingMonth.value + offset, 1)
  viewingYear.value = next.getFullYear()
  viewingMonth.value = next.getMonth()
  await loadDashboard()
}

onMounted(() => {
  setActivePageTitle('dashboard')
  loadDashboard()
})
</script>

<style scoped>
.dashboard-panel { border: 1px solid #e2e4d9; border-radius: 1rem; background: #fffdf6; box-shadow: 0 12px 32px rgba(54,66,48,.07); }
.dashboard-stat { border: 1px solid #e2e4d9; border-radius: 1rem; background: linear-gradient(145deg,#fffdf6,#f5f4eb); padding: 1.1rem 1.25rem; box-shadow: 0 10px 28px rgba(54,66,48,.06); }
.dashboard-stat-label { color: #728075; font-size: .72rem; font-weight: 600; letter-spacing: .04em; }
.dashboard-stat-icon { display: flex; height: 2rem; width: 2rem; align-items: center; justify-content: center; border-radius: .7rem; background: rgba(48,82,58,.07); font-size: .8rem; }
.dashboard-stat-value { margin-top: .65rem; color: #263d31; font-size: 1.45rem; font-weight: 600; letter-spacing: -.04em; }
.dashboard-stat-foot { margin-top: .35rem; color: #7b887c; font-size: .68rem; }
.dashboard-panel-title { color: #263d31; font-family: 'Playfair Display', Georgia, serif; font-size: 1rem; font-weight: 600; }
.dashboard-icon-button { display:flex; height:2.6rem; width:2.6rem; align-items:center; justify-content:center; border:1px solid #d9ddcf; border-radius:.8rem; color:#526b59; background:#fffdf6; }
.dashboard-icon-button:hover { background:#edf0e6; color:#17382d; }
.dashboard-action { display:inline-flex; align-items:center; border:1px solid #d9ddcf; border-radius:.8rem; background:#fffdf6; padding:.75rem 1rem; color:#3b6447; font-size:.75rem; font-weight:600; }
.dashboard-action:hover { color:#17382d; background:#edf0e6; }
.dashboard-donut { display:flex; height:11rem; width:11rem; align-items:center; justify-content:center; border-radius:50%; box-shadow:0 0 0 1px rgba(48,82,58,.1), 0 12px 30px rgba(54,66,48,.1); }
.dashboard-donut-center { display:flex; height:8.2rem; width:8.2rem; flex-direction:column; align-items:center; justify-content:center; border:1px solid #e3e5da; border-radius:50%; background:#fffdf6; }
.dashboard-donut-center span { color:#829084; font-size:.65rem; text-transform:uppercase; letter-spacing:.1em; }
.dashboard-donut-center strong { margin-top:.25rem; max-width:7rem; overflow:hidden; color:#263d31; font-size:1.15rem; text-overflow:ellipsis; white-space:nowrap; }
</style>
