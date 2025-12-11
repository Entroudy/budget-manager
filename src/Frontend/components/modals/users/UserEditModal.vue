<template>
  <Transition name="modal">
    <BaseModal v-if="showModal" :show-close="true" :showing="showModal" @close="close" :preventBackgroundClose="true">
      <template #title>
        <div class="flex items-center gap-3">
          <div class="w-10 h-10 rounded-full bg-primary/10 flex items-center justify-center">
            <i class="fas fa-user-edit text-primary text-lg"></i>
          </div>
          <div>
            <h2 class="text-xl font-semibold text-gray-900 dark:text-white">{{ $t('update') }} {{ $t('users') }}</h2>
            <p class="text-sm text-gray-500 dark:text-gray-400">{{ $t('edit_user_information') }}</p>
          </div>
        </div>
      </template>
      <template #body>
        <form v-enter-submit="submit" class="space-y-6">

          <!-- Personal Information Section -->
          <div class="space-y-4">
            <h3 class="text-sm font-semibold text-gray-700 dark:text-gray-300 uppercase tracking-wide">
              {{ $t('personal_information') }}
            </h3>

            <div class="grid grid-cols-1 md:grid-cols-2 gap-4">
              <!-- First Name -->
              <div>
                <label for="firstName" class="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-2">
                  {{ $t('first_name') }} <span class="text-red-500">*</span>
                </label>
                <input
                  id="firstName"
                  v-model="user.firstName"
                  type="text"
                  class="w-full px-4 py-3 rounded-lg border border-gray-300 dark:border-gray-600 bg-white dark:bg-gray-700 text-gray-900 dark:text-white placeholder-gray-400 focus:outline-none focus:ring-2 focus:ring-primary focus:border-transparent transition-all"
                  :class="{
                    'border-red-500 focus:ring-red-500': v$.firstName.$error,
                    'border-green-500 focus:ring-green-500': !v$.firstName.$invalid && v$.firstName.$dirty,
                  }"
                  @blur="v$.firstName.$touch"
                  :placeholder="$t('enter_first_name')"
                >
                <span v-if="v$.firstName.$error" class="text-xs text-red-500 mt-1 block">
                  {{ v$.firstName.$silentErrors[0].$message }}
                </span>
              </div>

              <!-- Last Name -->
              <div>
                <label for="lastName" class="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-2">
                  {{ $t('last_name') }} <span class="text-red-500">*</span>
                </label>
                <input
                  id="lastName"
                  v-model="user.lastName"
                  type="text"
                  class="w-full px-4 py-3 rounded-lg border border-gray-300 dark:border-gray-600 bg-white dark:bg-gray-700 text-gray-900 dark:text-white placeholder-gray-400 focus:outline-none focus:ring-2 focus:ring-primary focus:border-transparent transition-all"
                  :class="{
                    'border-red-500 focus:ring-red-500': v$.lastName.$error,
                    'border-green-500 focus:ring-green-500': !v$.lastName.$invalid && v$.lastName.$dirty,
                  }"
                  @blur="v$.lastName.$touch"
                  :placeholder="$t('enter_last_name')"
                >
                <span v-if="v$.lastName.$error" class="text-xs text-red-500 mt-1 block">
                  {{ v$.lastName.$silentErrors[0].$message }}
                </span>
              </div>
            </div>

            <!-- Nickname -->
            <div>
              <label for="nickName" class="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-2">
                {{ $t('nickName') }} <span class="text-red-500">*</span>
              </label>
              <input
                id="nickName"
                v-model="user.nickName"
                type="text"
                class="w-full px-4 py-3 rounded-lg border border-gray-300 dark:border-gray-600 bg-white dark:bg-gray-700 text-gray-900 dark:text-white placeholder-gray-400 focus:outline-none focus:ring-2 focus:ring-primary focus:border-transparent transition-all"
                :class="{
                  'border-red-500 focus:ring-red-500': v$.nickName.$error,
                  'border-green-500 focus:ring-green-500': !v$.nickName.$invalid && v$.nickName.$dirty,
                }"
                @blur="v$.nickName.$touch"
                :placeholder="$t('enter_nickname')"
              >
              <span v-if="v$.nickName.$error" class="text-xs text-red-500 mt-1 block">
                {{ v$.nickName.$silentErrors[0].$message }}
              </span>
            </div>

            <!-- Email -->
            <div>
              <label for="email" class="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-2">
                {{ $t('email') }} <span class="text-red-500">*</span>
              </label>
              <input
                id="email"
                v-model="user.email"
                type="email"
                class="w-full px-4 py-3 rounded-lg border border-gray-300 dark:border-gray-600 bg-white dark:bg-gray-700 text-gray-900 dark:text-white placeholder-gray-400 focus:outline-none focus:ring-2 focus:ring-primary focus:border-transparent transition-all"
                :class="{
                  'border-red-500 focus:ring-red-500': v$.email.$error,
                  'border-green-500 focus:ring-green-500': !v$.email.$invalid && v$.email.$dirty,
                }"
                @blur="v$.email.$touch"
                :placeholder="$t('enter_email')"
              >
              <span v-if="v$.email.$error" class="text-xs text-red-500 mt-1 block">
                {{ v$.email.$silentErrors[0].$message }}
              </span>
            </div>
          </div>

          <!-- Income Section -->
          <div class="space-y-4 pt-4 border-t border-gray-200 dark:border-gray-700">
            <div class="flex items-center justify-between">
              <div>
                <h3 class="text-sm font-semibold text-gray-700 dark:text-gray-300 uppercase tracking-wide">
                  {{ $t('income_sources') }}
                </h3>
                <p class="text-xs text-gray-500 dark:text-gray-400 mt-1">{{ $t('total_income') }}: {{ formatCurrency(totalIncome) }}</p>
              </div>
              <button
                type="button"
                @click="addIncome"
                class="flex items-center gap-2 px-3 py-1.5 text-sm bg-primary/10 text-primary rounded-lg hover:bg-primary/20 transition-colors"
              >
                <i class="fas fa-plus text-xs"></i>
                <span>{{ $t('add_income') }}</span>
              </button>
            </div>

            <!-- Income List -->
            <div v-if="user.incomes && user.incomes.length > 0" class="space-y-2">
              <div
                v-for="(income, index) in user.incomes"
                :key="income.id || `temp_${index}`"
                class="border border-gray-200 dark:border-gray-600 rounded-lg overflow-hidden transition-all"
                :class="expandedIncomeIndex === index ? 'bg-gray-50 dark:bg-gray-700' : 'bg-white dark:bg-gray-750'"
              >
                <!-- Collapsed View -->
                <div
                  @click="toggleIncomeExpand(index)"
                  class="p-3 cursor-pointer hover:bg-gray-50 dark:hover:bg-gray-700 transition-colors flex items-center justify-between"
                >
                  <div class="flex items-center gap-3 flex-1">
                    <div class="w-8 h-8 rounded-full flex items-center justify-center text-xs"
                         :class="income.paymentMethod === 1 ? 'bg-blue-100 dark:bg-blue-900 text-blue-600 dark:text-blue-300' : 'bg-green-100 dark:bg-green-900 text-green-600 dark:text-green-300'">
                      <i class="fas" :class="income.paymentMethod === 1 ? 'fa-credit-card' : 'fa-money-bill-wave'"></i>
                    </div>
                    <div class="flex-1">
                      <div class="flex items-center gap-2">
                        <span class="text-sm font-medium text-gray-900 dark:text-white capitalize">{{ income.paymentMethod === 1 ? $t('card') : $t('cash') }}</span>
                        <span class="text-xs px-2 py-0.5 rounded-full"
                              :class="income.id && !income.id.toString().startsWith('temp_') ? 'bg-green-100 dark:bg-green-900 text-green-700 dark:text-green-300' : 'bg-orange-100 dark:bg-orange-900 text-orange-700 dark:text-orange-300'">
                          {{ income.id && !income.id.toString().startsWith('temp_') ? $t('saved') : $t('not_saved') }}
                        </span>
                      </div>
                      <p v-if="income.description" class="text-xs text-gray-500 dark:text-gray-400 mt-0.5">{{ income.description }}</p>
                    </div>
                    <div class="text-right">
                      <span class="text-base font-bold text-gray-900 dark:text-white">{{ formatCurrency(income.amount || 0) }}</span>
                    </div>
                  </div>
                  <div class="ml-3">
                    <i class="fas fa-chevron-down text-gray-400 text-xs transition-transform" :class="expandedIncomeIndex === index ? 'rotate-180' : ''"></i>
                  </div>
                </div>

                <!-- Expanded Edit View -->
                <div v-if="expandedIncomeIndex === index" class="px-3 pb-3 pt-2 border-t border-gray-200 dark:border-gray-600">
                  <div class="space-y-3">
                    <div class="grid grid-cols-1 md:grid-cols-2 gap-3">
                      <!-- Payment Method -->
                      <div>
                        <label :for="`paymentMethod-${index}`" class="block text-xs font-medium text-gray-700 dark:text-gray-300 mb-1">
                          {{ $t('payment_method') }}
                        </label>
                        <select
                          :id="`paymentMethod-${index}`"
                          v-model.number="income.paymentMethod"
                          class="w-full px-3 py-2 rounded-lg border border-gray-300 dark:border-gray-600 bg-white dark:bg-gray-800 text-gray-900 dark:text-white text-sm focus:outline-none focus:ring-2 focus:ring-primary cursor-pointer"
                        >
                          <option :value="0">{{ $t('cash') }}</option>
                          <option :value="1">{{ $t('card') }}</option>
                        </select>
                      </div>

                      <!-- Amount -->
                      <div>
                        <label :for="`amount-${index}`" class="block text-xs font-medium text-gray-700 dark:text-gray-300 mb-1">
                          {{ $t('amount') }} (HUF)
                        </label>
                        <input
                          :id="`amount-${index}`"
                          v-model.number="income.amount"
                          type="number"
                          step="1"
                          class="w-full px-3 py-2 rounded-lg border border-gray-300 dark:border-gray-600 bg-white dark:bg-gray-800 text-gray-900 dark:text-white text-sm focus:outline-none focus:ring-2 focus:ring-primary"
                          :placeholder="$t('enter_amount')"
                        >
                      </div>
                    </div>

                    <!-- Description -->
                    <div>
                      <label :for="`description-${index}`" class="block text-xs font-medium text-gray-700 dark:text-gray-300 mb-1">
                        {{ $t('description') }} ({{ $t('optional') }})
                      </label>
                      <input
                        :id="`description-${index}`"
                        v-model="income.description"
                        type="text"
                        class="w-full px-3 py-2 rounded-lg border border-gray-300 dark:border-gray-600 bg-white dark:bg-gray-800 text-gray-900 dark:text-white text-sm focus:outline-none focus:ring-2 focus:ring-primary"
                        :placeholder="$t('enter_description')"
                      >
                    </div>

                    <!-- Action Buttons -->
                    <div class="flex gap-2 pt-2">
                      <button
                        type="button"
                        @click="saveIncome(income, index)"
                        :disabled="savingIncomeIndex === index"
                        class="flex items-center gap-2 px-3 py-2 text-sm bg-primary text-white rounded-lg hover:bg-primary-dark transition-colors disabled:opacity-50 flex-1"
                      >
                        <i v-if="savingIncomeIndex === index" class="fas fa-spinner fa-spin text-xs"></i>
                        <i v-else class="fas fa-save text-xs"></i>
                        <span>{{ $t('save') }}</span>
                      </button>
                      <button
                        type="button"
                        @click="deleteIncome(income, index)"
                        :disabled="deletingIncomeIndex === index"
                        class="flex items-center gap-2 px-3 py-2 text-sm bg-red-600 text-white rounded-lg hover:bg-red-700 transition-colors disabled:opacity-50"
                      >
                        <i v-if="deletingIncomeIndex === index" class="fas fa-spinner fa-spin text-xs"></i>
                        <i v-else class="fas fa-trash text-xs"></i>
                        <span>{{ $t('delete') }}</span>
                      </button>
                    </div>
                  </div>
                </div>
              </div>
            </div>

            <!-- Empty State -->
            <div v-else class="text-center py-8 border border-dashed border-gray-300 dark:border-gray-600 rounded-lg">
              <i class="fas fa-coins text-3xl text-gray-300 dark:text-gray-600 mb-2"></i>
              <p class="text-sm text-gray-500 dark:text-gray-400">{{ $t('no_income_sources_added') }}</p>
            </div>
          </div>

        </form>
      </template>
      <template #action>
        <div class="flex gap-3">
          <button
            type="button"
            @click="close"
            class="flex-1 px-6 py-3 rounded-lg border border-gray-300 dark:border-gray-600 text-gray-700 dark:text-gray-300 font-medium hover:bg-gray-50 dark:hover:bg-gray-700 transition-colors"
            :disabled="isLoading"
          >
            {{ $t('cancel') }}
          </button>
          <button
            @click="submit"
            class="flex-1 px-6 py-3 rounded-lg bg-primary text-white font-medium hover:bg-primary-dark transition-colors flex items-center justify-center gap-2"
            :disabled="isLoading"
          >
            <i v-if="isLoading" class="fas fa-spinner fa-spin"></i>
            <span>{{ $t('save') }}</span>
          </button>
        </div>
      </template>
    </BaseModal>
  </Transition>
</template>

<script lang="ts">
import { defineComponent } from 'vue'
import { required, helpers } from '@vuelidate/validators'
import { useVuelidate } from '@vuelidate/core'
import type { UserUpdate, UserListItem, Income } from '~/models/UserModel'
import { PaymentMethod } from '~/models/UserModel'
import { useUserStore } from '~/stores/UserStore'
import { useIncomeStore } from '~/stores/IncomeStore'

export default defineComponent({
  name: 'UserEditModal',
  emits: ['item-edited'],
  setup(props, context) {
    const user = reactive({
      id: '',
      lastName: '',
      firstName: '',
      nickName: '',
      email: '',
      password: '',
      confirmPassword: '',
      incomes: [] as Income[],
    } as UserUpdate)

    const { t } = useI18n();
    const { $showNotification } = useNuxtApp()

    const isLoading = ref(false)
    const showModal = ref(false)
    const savingIncomeIndex = ref<number | null>(null)
    const deletingIncomeIndex = ref<number | null>(null)
    const expandedIncomeIndex = ref<number | null>(null)

    const { updateUser } = useUserStore()
    const { getIncomesByUserId, createIncome, updateIncome, deleteIncome: deleteIncomeFromStore } = useIncomeStore()

    const rules = computed(() => {
      return {
        lastName: {
          required: helpers.withMessage('The last name field is required', required)
        },
        firstName: {
          required: helpers.withMessage('The first name field is required', required)
        },
        nickName: {
          required: helpers.withMessage('The nickName field is required', required)
        },
        email: {
          required: helpers.withMessage('The email field is required', required)
        }
      }
    })
    const v$ = useVuelidate(rules, user)

    // Income Management
    const toggleIncomeExpand = (index: number) => {
      if (expandedIncomeIndex.value === index) {
        expandedIncomeIndex.value = null
      } else {
        expandedIncomeIndex.value = index
      }
    }

    const addIncome = () => {
      if (!user.incomes) {
        user.incomes = []
      }
      user.incomes.push({
        paymentMethod: PaymentMethod.Card, // Card = 1
        amount: 0,
        description: ''
      })
      // Auto-expand the newly added income
      expandedIncomeIndex.value = user.incomes.length - 1
    }

    const saveIncome = async (income: Income, index: number) => {
      savingIncomeIndex.value = index
      try {
        if (income.id && !income.id.toString().startsWith('temp_')) {
          // Update existing income
          const response = await updateIncome(income.id, income, user.id)
          if (response) {
            $showNotification('success', t('successful_save'))
            await loadIncomes()
            expandedIncomeIndex.value = null
            // Emit event to refresh user cards
            context.emit('item-edited')
          }
        } else {
          // Create new income
          const response = await createIncome(income, user.id)
          if (response) {
            $showNotification('success', t('successful_save'))
            await loadIncomes()
            expandedIncomeIndex.value = null
            // Emit event to refresh user cards
            context.emit('item-edited')
          }
        }
      } catch (error) {
        console.error('Error saving income:', error)
      } finally {
        savingIncomeIndex.value = null
      }
    }

    const deleteIncome = async (income: Income, index: number) => {
      if (!income.id || income.id.toString().startsWith('temp_')) {
        // Just remove from list if not saved yet
        user.incomes.splice(index, 1)
        expandedIncomeIndex.value = null
        return
      }

      deletingIncomeIndex.value = index
      try {
        const response = await deleteIncomeFromStore(income.id)
        if (response !== null) {
          $showNotification('success', t('successful_delete'))
          await loadIncomes()
          expandedIncomeIndex.value = null
          // Emit event to refresh user cards
          context.emit('item-edited')
        }
      } catch (error) {
        console.error('Error deleting income:', error)
      } finally {
        deletingIncomeIndex.value = null
      }
    }

    const loadIncomes = async () => {
      const incomes = await getIncomesByUserId(user.id)
      if (incomes) {
        user.incomes = incomes
      }
    }

    const totalIncome = computed(() => {
      if (!user.incomes || user.incomes.length === 0) return 0
      return user.incomes.reduce((sum, income) => sum + (income.amount || 0), 0)
    })

    const formatCurrency = (amount: number) => {
      return new Intl.NumberFormat('hu-HU', {
        style: 'currency',
        currency: 'HUF',
        minimumFractionDigits: 0,
        maximumFractionDigits: 0
      }).format(amount)
    }

    const open = async (userData: UserListItem) => {
      showModal.value = true
      Object.assign(user, {
        ...userData,
        incomes: []
      });
      // Load incomes from API
      await loadIncomes()
    }

    const close = () => {
      showModal.value = false
      v$.value.$reset()
      resetUserForm()
    }

    const resetUserForm = () => {
      user.lastName = ''
      user.firstName = ''
      user.nickName = ''
      user.email = ''
      user.incomes = []
    }

    const submit = async (e: MouseEvent) => {
      if (e) e.preventDefault()
      isLoading.value = true
      v$.value.$validate()
      if (v$.value.$error) isLoading.value = false
      if (!v$.value.$error) {
        const response = await updateUser(user.id, user)
        isLoading.value = false
        if (!response) { return }
        context.emit('item-edited')
        v$.value.$reset()
        close()
      }
    }

    return {
      v$,
      rules,
      user,
      showModal,
      open,
      close,
      submit,
      isLoading,
      addIncome,
      saveIncome,
      deleteIncome,
      toggleIncomeExpand,
      totalIncome,
      formatCurrency,
      savingIncomeIndex,
      deletingIncomeIndex,
      expandedIncomeIndex
    }
  }
})
</script>
