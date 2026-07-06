<template>
  <Transition name="modal">
    <BaseModal v-if="showModal" :show-close="true" :showing="showModal" @close="close" :preventBackgroundClose="true">
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

            <div class="sm:col-span-full">
              <label for="item" class="required capitalize-block">{{ $t('item') }}</label>
              <input id="item" v-model="spending.item" type="text" class="w-full" :class="{
                '!border-red-600 !focus-visible:border-red-600': v$.item.$error,
                '!border-green-500': !v$.item.$invalid,
              }" @change="v$.item.$touch">
              <span v-if="v$.item.$error" class="text-xs text-red-600">{{ v$.item.$silentErrors[0].$message }}</span>
            </div>

            <div class="sm:col-span-3">
              <label for="amount" class="required capitalize-block">{{ $t('amount') }}</label>
              <input id="amount" v-model.number="spending.amount" type="number" step="0.01" class="w-full" :class="{
                '!border-red-600 !focus-visible:border-red-600': v$.amount.$error,
                '!border-green-500': !v$.amount.$invalid,
              }" @change="v$.amount.$touch">
              <span v-if="v$.amount.$error" class="text-xs text-red-600">{{ v$.amount.$silentErrors[0].$message }}</span>
            </div>

            <div class="sm:col-span-3">
              <label for="date" class="required capitalize-block">{{ $t('date') }}</label>
              <input id="date" v-model="spending.date" type="date" class="w-full" :class="{
                '!border-red-600 !focus-visible:border-red-600': v$.date.$error,
                '!border-green-500': !v$.date.$invalid,
              }" @change="v$.date.$touch">
              <span v-if="v$.date.$error" class="text-xs text-red-600">{{ v$.date.$silentErrors[0].$message }}</span>
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
              <label for="color" class="capitalize-block">{{ $t('color') }}</label>
              <div class="flex gap-2">
                <button
                  v-for="colorOption in colorOptions"
                  :key="colorOption.value"
                  type="button"
                  @click="spending.color = colorOption.value"
                  class="w-12 h-10 rounded border-2 transition-all"
                  :class="spending.color === colorOption.value ? 'border-gray-900 dark:border-white scale-110' : 'border-gray-300 dark:border-gray-600 hover:scale-105'"
                  :style="{ backgroundColor: colorOption.value }"
                  :title="colorOption.label"
                ></button>
              </div>
            </div>

            <div class="sm:col-span-full">
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
import { required, helpers, minValue } from '@vuelidate/validators'
import { useVuelidate } from '@vuelidate/core'
import type { SpendingCreate } from '~/models/SpendingModel'
import { PaymentMethod } from '~/models/SpendingModel'
import { useSpendingStore } from '~/stores/SpendingStore'
import { useAuthStore } from '~/stores/AuthStore'
import { useUserStore } from '~/stores/UserStore'
import { storeToRefs } from 'pinia'

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

      const spendingMonth = getMonthValue(spending.date || getCurrentMonthValue())
      return selectedUser.incomes.some((income) =>
        getMonthValue(income.date || '') === spendingMonth && income.paymentMethod === PaymentMethod.Cash
      )
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
      await getUsers({ page_number: 1, page_size: 999 })
      spending.applicationUserId = users.value.find(user => user.email === account.value.email)?.id || ''
      spending.paymentMethod = PaymentMethod.Card
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
      const isFormCorrect = await v$.value.$validate()
      if (!isFormCorrect) return

      isLoading.value = true
      try {
        const response = await createSpending(spending)
        if (response) {
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
