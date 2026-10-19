<template>
  <Transition name="modal">
    <BaseModal v-if="showModal" :show-close="true" :showing="showModal" @close="close" :preventBackgroundClose="true" max-width="md:max-w-7xl">
      <template #title>
        <span class="break-normal">
          {{ $t('create') }}
        </span>
      </template>
      <template #body>
        <form v-enter-submit="submit">
          <div class="grid grid-cols-1 gap-x-6 gap-y-8 sm:grid-cols-6">
            <div class="sm:col-span-full">
              <label for="user" class="required capitalize-block">{{ $t('user') }}</label>
              <select id="user" v-model="spending.applicationUserId" class="w-full" :class="{
                '!border-red-600 !focus-visible:border-red-600': v$.applicationUserId.$error,
                '!border-green-500': !v$.applicationUserId.$invalid,
              }" @change="v$.applicationUserId.$touch">
                <option value="">{{ $t('select_user') }}</option>
                <option v-for="user in users" :key="user.id" :value="user.id">
                  {{ user.nickName || user.lastName }}
                </option>
              </select>
              <span v-if="v$.applicationUserId.$error" class="text-xs text-red-600">{{ v$.applicationUserId.$silentErrors[0].$message }}</span>
            </div>

            <div class="sm:col-span-full border-b border-[#e8e8e1] pb-3">
              <div>
                <h3 class="font-display text-lg font-semibold text-[#244536]">{{ $t('spendings') }}</h3>
                <p class="mt-1 text-sm text-gray-500">{{ $t('spending_entry_hint') }}</p>
              </div>
            </div>
            <div v-for="(entry, index) in bulkItems" :key="index" class="sm:col-span-full grid grid-cols-1 sm:grid-cols-2 xl:grid-cols-12 gap-3 items-end border-b border-gray-100 py-4">
                <div class="sm:col-span-2 xl:col-span-3"><label>{{ $t('item') }}</label><input v-model="entry.item" class="w-full" type="text"></div>
                <div class="xl:col-span-2"><label>{{ $t('amount') }}</label><div class="flex gap-2"><input v-model.number="entry.amount" class="w-full min-w-0" type="number" min="0.01" step="0.01"><button type="button" class="inline-flex h-10 w-10 shrink-0 items-center justify-center rounded-lg bg-[#315f49] text-white transition hover:bg-[#244536]" :aria-label="$t('add_another')" :title="$t('add_another')" @click="addBulkItem"><i class="fas fa-plus" /></button></div></div>
                <div class="xl:col-span-2"><label>{{ $t('currency') }}</label><select v-model="entry.currency" class="w-full"><option value="HUF">HUF</option><option value="EUR">EUR</option><option value="USD">USD</option></select><p v-if="entry.currency !== 'HUF'" class="mt-1 text-[10px] text-gray-500">1 {{ entry.currency }} = {{ entry.currency === 'EUR' ? eurRate : usdRate }} HUF</p></div>
                <div class="xl:col-span-2"><label>{{ $t('date') }}</label><input v-model="entry.date" class="w-full" type="date"></div>
                <div class="sm:col-span-1 xl:col-span-2"><label>{{ $t('color') }}</label><div class="flex items-center gap-2 h-9">
                  <button v-for="option in colorOptions" :key="option.value" type="button" class="w-6 h-6 rounded-full border-2" :class="entry.color === option.value ? 'border-gray-900 scale-110' : 'border-white shadow'" :style="{ backgroundColor: option.value }" :aria-label="option.label" @click="entry.color = option.value" />
                </div></div>
                <button v-if="bulkItems.length > 1" type="button" class="xl:col-span-1 h-9 text-red-700 hover:bg-red-50 rounded-lg" :aria-label="$t('remove')" @click="bulkItems.splice(index, 1)"><i class="fas fa-trash" /></button>
            </div>

            <div class="sm:col-span-3">
              <label for="paymentMethod" class="required capitalize-block">{{ $t('payment_method') }}</label>
              <select id="paymentMethod" v-model.number="spending.paymentMethod" class="w-full" :class="{
                '!border-red-600 !focus-visible:border-red-600': v$.paymentMethod.$error,
                '!border-green-500': !v$.paymentMethod.$invalid,
              }" @change="v$.paymentMethod.$touch">
                <option :value="PaymentMethod.Cash">{{ $t('cash') }}</option>
                <option :value="PaymentMethod.Card">{{ $t('card') }}</option>
              </select>
              <span v-if="v$.paymentMethod.$error" class="text-xs text-red-600">{{ v$.paymentMethod.$silentErrors[0].$message }}</span>
            </div>

            <div
              v-if="showCashWarning"
              class="sm:col-span-full rounded-lg border border-amber-300 bg-amber-50 dark:border-amber-700 dark:bg-amber-900/20 p-4"
            >
              <div class="flex items-start gap-3">
                <i class="fas fa-triangle-exclamation text-amber-500 mt-1"></i>
                <div class="flex-1">
                  <p class="font-semibold text-amber-800 dark:text-amber-200">
                    {{ $t('cash_spending_warning_title') }}
                  </p>
                  <p class="text-sm text-amber-700 dark:text-amber-300 mt-1">
                    {{ $t('cash_spending_warning_message') }}
                  </p>
                  <div class="flex gap-2 mt-3">
                    <button
                      type="button"
                      class="px-3 py-2 rounded-md bg-amber-600 text-white text-sm font-medium hover:bg-amber-700 transition-colors"
                      @click="confirmCashWarning"
                    >
                      {{ $t('confirmation') }}
                    </button>
                    <button
                      type="button"
                      class="px-3 py-2 rounded-md border border-amber-300 text-amber-800 dark:text-amber-200 text-sm font-medium hover:bg-amber-100 dark:hover:bg-amber-900/40 transition-colors"
                      @click="dismissCashWarning"
                    >
                      {{ $t('cancel') }}
                    </button>
                  </div>
                </div>
              </div>
            </div>

            <div class="sm:col-span-3">
              <label for="comment" class="capitalize-block">{{ $t('comment') }}</label>
              <textarea id="comment" v-model="spending.comment" rows="3" class="w-full"></textarea>
            </div>

          </div>
        </form>
      </template>
      <template #action>
        <button class="btn bg-primary w-full text-white capitalize" @click="submit" :disabled="isLoading">
          {{ $t('create') }}<i v-show="isLoading" class="fas fa-spinner fa-spin text-white ml-2"></i>
        </button>
      </template>
    </BaseModal>
  </Transition>
</template>

<script lang="ts">
import { defineComponent } from 'vue'
import { required, helpers } from '@vuelidate/validators'
import { useVuelidate } from '@vuelidate/core'
import type { SpendingCreate } from '~/models/SpendingModel'
import { PaymentMethod } from '~/models/SpendingModel'
import { useSpendingStore } from '~/stores/SpendingStore'
import { useAuthStore } from '~/stores/AuthStore'
import { useUserStore } from '~/stores/UserStore'
import { ALL_RECORDS_PAGE_SIZE } from '~/core/pagination'
import { storeToRefs } from 'pinia'
import { loadSystemPreferences } from '~/utils/appPreferences'

export default defineComponent({
  name: 'SpendingCreateModal',
  emits: ['item-created'],
  setup(props, context) {
    const { users } = storeToRefs(useUserStore())
    const { account } = storeToRefs(useAuthStore())
    const { getUsers } = useUserStore()

    const spending = reactive({
      applicationUserId: '',
      item: '',
      amount: 0,
      paymentMethod: PaymentMethod.Card,
      comment: '',
      color: '#6B7280',
      date: new Date().toISOString().split('T')[0],
    } as SpendingCreate)

    const { t } = useI18n()
    const colorOptions = computed(() => [
      { label: t('gray'), value: '#6B7280' },
      { label: t('red'), value: '#EF4444' },
      { label: t('yellow'), value: '#EAB308' },
      { label: t('light_blue'), value: '#3B82F6' },
      { label: t('dark_blue'), value: '#1E3A8A' }
    ])
    const isLoading = ref(false)
    const showModal = ref(false)
    const showCashWarning = ref(false)
    const eurRate = ref(400)
    const usdRate = ref(370)
    const createBulkItem = () => ({ item: '', amount: 0, currency: 'HUF', date: new Date().toISOString().split('T')[0], color: '#6B7280' })
    const bulkItems = ref([createBulkItem()])
    const addBulkItem = () => bulkItems.value.push(createBulkItem())
    const bypassCashWarning = ref(false)

    const getMonthValue = (dateString: string) => dateString.slice(0, 7)
    const getCurrentMonthValue = () => {
      const today = new Date()
      const year = today.getFullYear()
      const month = String(today.getMonth() + 1).padStart(2, '0')
      return `${year}-${month}`
    }

    const currentUserHasCashIncomeForMonth = computed(() => {
      const selectedUser = users.value.find(user => user.id === spending.applicationUserId)
      if (!selectedUser || !selectedUser.incomes || selectedUser.incomes.length === 0) {
        return false
      }

      const spendingMonths = bulkItems.value.map(item => getMonthValue(item.date || getCurrentMonthValue()))
      return spendingMonths.every(month => selectedUser.incomes!.some((income) =>
        getMonthValue(income.date || '') === month && income.paymentMethod === PaymentMethod.Cash
      ))
    })

    const shouldShowCashWarning = computed(() => {
      return spending.paymentMethod === PaymentMethod.Cash &&
        !!spending.applicationUserId &&
        !!spending.date &&
        !currentUserHasCashIncomeForMonth.value
    })

    const { createSpending } = useSpendingStore()
    const { $showNotification } = useNuxtApp()

    const rules = computed(() => {
      return {
        applicationUserId: {
          required: helpers.withMessage(t('user_required'), required)
        },
        item: {
          required: helpers.withMessage(t('item_required'), required)
        },
        amount: {
          required: helpers.withMessage(t('amount_required'), required),
          //minValue: helpers.withMessage(t('amount_must_be_positive'), minValue(0.01))
        },
        paymentMethod: {
          required: helpers.withMessage(t('payment_method_required'), required)
        },
        date: {
          required: helpers.withMessage(t('date_required'), required)
        }
      }
    })

    const v$ = useVuelidate(rules, spending)

    watch(
      () => [spending.applicationUserId, spending.paymentMethod, spending.date],
      () => {
        showCashWarning.value = false
        bypassCashWarning.value = false
      }
    )

    const open = async () => {
      await getUsers({ page_number: 1, page_size: ALL_RECORDS_PAGE_SIZE })
      spending.applicationUserId = users.value.find(user => user.email === account.value.email)?.id || ''
      if (typeof localStorage !== 'undefined') {
        try {
          const rates = JSON.parse(localStorage.getItem('budget-manager-currency-rates') || '{}')
          eurRate.value = Number(rates.eur) || 400
          usdRate.value = Number(rates.usd) || 370
        } catch { /* use defaults */ }
      }
      spending.paymentMethod = loadSystemPreferences().defaultPaymentMethod
      showCashWarning.value = false
      bypassCashWarning.value = false
      showModal.value = true
    }

    const close = () => {
      showModal.value = false
      resetForm()
    }

    const resetForm = () => {
      spending.applicationUserId = ''
      spending.item = ''
      spending.amount = 0
      bulkItems.value = [createBulkItem()]
      spending.paymentMethod = PaymentMethod.Card
      spending.comment = ''
      spending.color = '#6B7280'
      spending.date = new Date().toISOString().split('T')[0]
      showCashWarning.value = false
      bypassCashWarning.value = false
      v$.value.$reset()
    }

    const confirmCashWarning = async () => {
      bypassCashWarning.value = true
      showCashWarning.value = false
      await submit()
    }

    const dismissCashWarning = () => {
      showCashWarning.value = false
      bypassCashWarning.value = false
    }

    const performSubmit = async () => {
      if (!spending.applicationUserId || bulkItems.value.length === 0) return

      isLoading.value = true
      try {
        const entries = bulkItems.value
        const toHuf = (amount: number, selectedCurrency: string) => Math.round(amount * (selectedCurrency === 'EUR' ? eurRate.value : selectedCurrency === 'USD' ? usdRate.value : 1))
        for (const entry of entries) {
          if (!entry.item.trim() || !Number.isFinite(entry.amount) || entry.amount <= 0 || !entry.date) {
            $showNotification('error', t('bulk_items_required'))
            return
          }
        }
        for (const entry of entries) {
          const response = await createSpending({ ...spending, item: entry.item.trim(), date: entry.date, color: entry.color, amount: toHuf(entry.amount, entry.currency) })
          if (!response) throw new Error('Spending creation failed')
        }
        if (entries.length) {
          $showNotification('success', t('spending_created_successfully'))
          context.emit('item-created')
          close()
        }
      } catch (error) {
        console.error('Error creating spending:', error)
      } finally {
        isLoading.value = false
        bypassCashWarning.value = false
      }
    }

    const submit = async () => {
      const shouldWarn = shouldShowCashWarning.value && !bypassCashWarning.value
      if (shouldWarn) {
        showCashWarning.value = true
        return
      }

      showCashWarning.value = false
      await performSubmit()
    }

    return {
      users,
      spending,
      colorOptions,
      eurRate, usdRate, bulkItems, createBulkItem, addBulkItem,
      v$,
      isLoading,
      showModal,
      showCashWarning,
      PaymentMethod,
      open,
      close,
      confirmCashWarning,
      dismissCashWarning,
      submit,
    }
  },
})
</script>
