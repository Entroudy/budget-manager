<template>
  <section class="mx-auto max-w-5xl text-[#253d32]">
    <header class="mb-8">
      <h2 class="mt-1 font-display text-3xl font-semibold text-[#17382d]">{{ $t('settings') }}</h2>
      <nav class="mt-6 flex gap-8 border-b border-[#dedfd6]" :aria-label="$t('settings')">
        <button type="button" class="border-b-2 px-1 pb-3 text-sm font-semibold transition" :class="activeTab === 'currency' ? 'border-[#55735b] text-[#355b43]' : 'border-transparent text-[#84917f] hover:text-[#355b43]'" @click="activeTab = 'currency'">{{ $t('currency_settings') }}</button>
        <button type="button" class="border-b-2 px-1 pb-3 text-sm font-semibold transition" :class="activeTab === 'system' ? 'border-[#55735b] text-[#355b43]' : 'border-transparent text-[#84917f] hover:text-[#355b43]'" @click="activeTab = 'system'">{{ $t('system_settings') }}</button>
      </nav>
    </header>

    <div v-if="activeTab === 'currency'" class="rounded-2xl border border-[#ecece5] bg-white p-6 shadow-sm sm:p-9">
      <div class="max-w-3xl">
        <h3 class="font-display text-xl font-semibold text-[#223b30]">{{ $t('currency_settings') }}</h3>
        <p class="mt-2 text-sm leading-6 text-[#737d72]">{{ $t('currency_settings_help') }}</p>
      </div>

      <form class="mt-8 grid w-full grid-cols-1 gap-x-6 gap-y-7 sm:grid-cols-2" @submit.prevent="saveRates">
        <div>
          <label for="eur-rate" class="mb-2 block text-sm font-semibold text-[#34463b]">{{ $t('euro_exchange_rate') }}</label>
          <div class="relative">
            <input id="eur-rate" v-model.number="eur" class="w-full rounded-lg border border-[#d9ddd5] bg-white px-4 py-3 pr-16 text-[#23372c] shadow-sm outline-none transition focus:border-[#6b8b70] focus:ring-2 focus:ring-[#6b8b70]/20" type="number" min="0.01" step="0.01" required>
            <span class="pointer-events-none absolute inset-y-0 right-4 flex items-center text-sm text-[#7b857b]">HUF / EUR</span>
          </div>
          <p class="mt-2 text-xs text-[#858d83]">1 EUR = {{ eur || 0 }} HUF</p>
        </div>

        <div>
          <label for="usd-rate" class="mb-2 block text-sm font-semibold text-[#34463b]">{{ $t('usd_exchange_rate') }}</label>
          <div class="relative">
            <input id="usd-rate" v-model.number="usd" class="w-full rounded-lg border border-[#d9ddd5] bg-white px-4 py-3 pr-16 text-[#23372c] shadow-sm outline-none transition focus:border-[#6b8b70] focus:ring-2 focus:ring-[#6b8b70]/20" type="number" min="0.01" step="0.01" required>
            <span class="pointer-events-none absolute inset-y-0 right-4 flex items-center text-sm text-[#7b857b]">HUF / USD</span>
          </div>
          <p class="mt-2 text-xs text-[#858d83]">1 USD = {{ usd || 0 }} HUF</p>
        </div>

        <div class="border-t border-[#ecece5] pt-5 sm:col-span-2">
          <button class="rounded-lg bg-[#315d45] px-6 py-2.5 text-sm font-semibold text-white transition hover:bg-[#264b38]" type="submit">{{ $t('save') }}</button>
        </div>
      </form>
    </div>

    <div v-else class="rounded-2xl border border-[#ecece5] bg-white p-6 shadow-sm sm:p-9">
      <div>
        <h3 class="font-display text-xl font-semibold text-[#223b30]">{{ $t('system_settings') }}</h3>
        <p class="mt-2 text-sm leading-6 text-[#737d72]">{{ $t('system_settings_help') }}</p>
      </div>

      <form class="mt-8 space-y-8" @submit.prevent="saveSystem">
        <section>
          <h4 class="text-sm font-semibold text-[#34463b]">{{ $t('default_payment_method') }}</h4>
          <p class="mt-1 text-sm text-[#858d83]">{{ $t('default_payment_method_help') }}</p>
          <div class="mt-4 inline-flex rounded-lg border border-[#dfe2d8] bg-[#f5f6f2] p-1">
            <button type="button" class="rounded-md px-4 py-2 text-sm font-semibold transition" :class="defaultPaymentMethod === PaymentMethod.Card ? 'bg-white text-[#315d45] shadow-sm' : 'text-[#899187] hover:text-[#315d45]'" :aria-pressed="defaultPaymentMethod === PaymentMethod.Card" @click="defaultPaymentMethod = PaymentMethod.Card">{{ $t('card') }}</button>
            <button type="button" class="rounded-md px-4 py-2 text-sm font-semibold transition" :class="defaultPaymentMethod === PaymentMethod.Cash ? 'bg-white text-[#315d45] shadow-sm' : 'text-[#899187] hover:text-[#315d45]'" :aria-pressed="defaultPaymentMethod === PaymentMethod.Cash" @click="defaultPaymentMethod = PaymentMethod.Cash">{{ $t('cash') }}</button>
          </div>
        </section>

        <section class="border-t border-[#ecece5] pt-7">
          <h4 class="text-sm font-semibold text-[#34463b]">{{ $t('spending_list_defaults') }}</h4>
          <div class="mt-5 grid gap-6 sm:grid-cols-2">
            <div>
              <p class="mb-3 text-sm text-[#737d72]">{{ $t('default_view') }}</p>
              <div class="inline-flex rounded-lg border border-[#dfe2d8] bg-[#f5f6f2] p-1" :aria-label="$t('view_mode')">
                <button type="button" class="flex h-10 items-center gap-2 rounded-md px-3 text-sm font-semibold transition" :class="spendingViewMode === 'list' ? 'bg-white text-[#315d45] shadow-sm' : 'text-[#899187] hover:text-[#315d45]'" :aria-pressed="spendingViewMode === 'list'" @click="spendingViewMode = 'list'"><i class="fas fa-list" />{{ $t('list_view') }}</button>
                <button type="button" class="flex h-10 items-center gap-2 rounded-md px-3 text-sm font-semibold transition" :class="spendingViewMode === 'grid' ? 'bg-white text-[#315d45] shadow-sm' : 'text-[#899187] hover:text-[#315d45]'" :aria-pressed="spendingViewMode === 'grid'" @click="spendingViewMode = 'grid'"><i class="fas fa-th-large" />{{ $t('grid_view') }}</button>
              </div>
            </div>
            <div>
              <p class="mb-3 text-sm text-[#737d72]">{{ $t('items_per_page') }}</p>
              <div class="inline-flex rounded-lg border border-[#dfe2d8] bg-[#f5f6f2] p-1" :aria-label="$t('items_per_page')">
                <button v-for="size in pageSizeOptions" :key="size" type="button" class="min-w-10 rounded-md px-3 py-2 text-sm font-semibold transition" :class="spendingPageSize === size ? 'bg-white text-[#315d45] shadow-sm' : 'text-[#899187] hover:text-[#315d45]'" :aria-pressed="spendingPageSize === size" @click="spendingPageSize = size">{{ size }}</button>
              </div>
            </div>
          </div>
        </section>

        <div class="border-t border-[#ecece5] pt-5">
          <button class="rounded-lg bg-[#315d45] px-6 py-2.5 text-sm font-semibold text-white transition hover:bg-[#264b38]" type="submit">{{ $t('save') }}</button>
        </div>
      </form>
    </div>
  </section>
</template>

<script setup lang="ts">
import { useGlobalStore } from '~/stores/global'
import { PaymentMethod } from '~/models/SpendingModel'
import { loadSystemPreferences, saveSystemPreferences, type SpendingViewMode } from '~/utils/appPreferences'

definePageMeta({ middleware: 'auth' })
const { setActivePageTitle } = useGlobalStore()
const { t } = useI18n()
const eur = ref(400)
const usd = ref(370)
const activeTab = ref<'currency' | 'system'>('currency')
const defaultPaymentMethod = ref(PaymentMethod.Card)
const spendingViewMode = ref<SpendingViewMode>('list')
const pageSizeOptions = [10, 25, 50, 100]
const spendingPageSize = ref(50)
onMounted(() => {
  setActivePageTitle('settings')
  try {
    const rates = JSON.parse(localStorage.getItem('budget-manager-currency-rates') || '{}')
    eur.value = Number(rates.eur) || 400
    usd.value = Number(rates.usd) || 370
  } catch { /* use defaults */ }
  const preferences = loadSystemPreferences()
  defaultPaymentMethod.value = preferences.defaultPaymentMethod
  spendingViewMode.value = preferences.spendingViewMode
  spendingPageSize.value = preferences.spendingPageSize
})
const saveRates = () => {
  if (eur.value <= 0 || usd.value <= 0) return
  localStorage.setItem('budget-manager-currency-rates', JSON.stringify({ eur: eur.value, usd: usd.value }))
  useNuxtApp().$showNotification('success', t('successful_save'))
}
const saveSystem = () => {
  saveSystemPreferences({
    defaultPaymentMethod: defaultPaymentMethod.value,
    spendingViewMode: spendingViewMode.value,
    spendingPageSize: spendingPageSize.value,
  })
  useNuxtApp().$showNotification('success', t('successful_save'))
}
</script>
