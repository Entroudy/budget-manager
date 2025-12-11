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
import type { UserUpdate, UserListItem } from '~/models/UserModel'
import { useUserStore } from '~/stores/UserStore'

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
    } as UserUpdate)

    const { t } = useI18n();

    const isLoading = ref(false)
    const showModal = ref(false)

    const { updateUser } = useUserStore()

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

    const open = (userData: UserListItem) => {
      showModal.value = true
      console.log(userData)
      Object.assign(user, userData);
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
      isLoading
    }
  }
})
</script>
