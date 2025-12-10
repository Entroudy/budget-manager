<template>
  <Transition name="modal">
    <BaseModal v-if="showModal" :show-close="true" :showing="showModal" @close="close" :preventBackgroundClose="true">
      <template #title>
        <span class="break-normal">
          {{ $t('create_spending') }}
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

            <div class="sm:col-span-3">
              <label for="color" class="capitalize-block">{{ $t('color') }}</label>
              <input id="color" v-model="spending.color" type="color" class="w-full h-10">
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
import { useUserStore } from '~/stores/UserStore'
import { storeToRefs } from 'pinia'

export default defineComponent({
  name: 'SpendingCreateModal',
  emits: ['item-created'],
  setup(props, context) {
    const { users } = storeToRefs(useUserStore())
    const { getUsers } = useUserStore()

    const spending = reactive({
      applicationUserId: '',
      item: '',
      amount: 0,
      paymentMethod: PaymentMethod.Cash,
      comment: '',
      color: '#3B82F6',
      date: new Date().toISOString().split('T')[0],
    } as SpendingCreate)

    const { t } = useI18n()
    const isLoading = ref(false)
    const showModal = ref(false)

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
          minValue: helpers.withMessage(t('amount_must_be_positive'), minValue(0.01))
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

    const open = async () => {
      await getUsers({ page_number: 1, page_size: 999 })
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
      spending.paymentMethod = PaymentMethod.Cash
      spending.comment = ''
      spending.color = '#3B82F6'
      spending.date = new Date().toISOString().split('T')[0]
      v$.value.$reset()
    }

    const submit = async () => {
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
      }
    }

    return {
      users,
      spending,
      v$,
      isLoading,
      showModal,
      PaymentMethod,
      open,
      close,
      submit,
    }
  },
})
</script>
