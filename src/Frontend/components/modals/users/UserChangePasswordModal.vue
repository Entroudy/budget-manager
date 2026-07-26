<template>
  <Transition name="modal">
    <BaseModal v-if="showModal" :show-close="true" :showing="showModal" @close="close" :preventBackgroundClose="true">
      <template #title>
        <span class="break-normal capitalize-block">
          {{ $t('change_password') }}
        </span>
      </template>

      <template #body>
        <form v-enter-submit="submit" class="space-y-6">
          <div class="grid grid-cols-1 gap-4 sm:grid-cols-2">
            <div>
              <label for="password" class="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-2">
                {{ $t('password') }} <span class="text-red-500">*</span>
              </label>
              <div class="relative">
                <input
                  id="password"
                  v-model="user.password"
                  :type="passwordVisible ? 'text' : 'password'"
                  class="w-full px-4 py-3 pr-12 rounded-lg border border-gray-300 dark:border-gray-600 bg-white dark:bg-gray-700 text-gray-900 dark:text-white placeholder-gray-400 focus:outline-none focus:ring-2 focus:ring-primary focus:border-transparent transition-all"
                  :class="{
                    'border-red-500 focus:ring-red-500': v$.password.$error,
                    'border-green-500 focus:ring-green-500': !v$.password.$invalid && v$.password.$dirty,
                  }"
                  @blur="v$.password.$touch"
                  :placeholder="$t('enter_password')"
                >
                <button
                  type="button"
                  class="absolute right-3 top-1/2 -translate-y-1/2 text-gray-500 hover:text-gray-700 dark:text-gray-400 dark:hover:text-gray-200 transition-colors"
                  @click="passwordVisible = !passwordVisible"
                >
                  <i class="fas" :class="passwordVisible ? 'fa-eye-slash' : 'fa-eye'"></i>
                </button>
              </div>
              <span v-if="v$.password.$error" class="text-xs text-red-500 mt-1 block">
                {{ v$.password.$silentErrors[0].$message }}
              </span>
            </div>

            <div>
              <label for="confirmPassword" class="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-2">
                {{ $t('confirm_password') }} <span class="text-red-500">*</span>
              </label>
              <div class="relative">
                <input
                  id="confirmPassword"
                  v-model="user.confirmPassword"
                  :type="passwordVisible ? 'text' : 'password'"
                  class="w-full px-4 py-3 pr-12 rounded-lg border border-gray-300 dark:border-gray-600 bg-white dark:bg-gray-700 text-gray-900 dark:text-white placeholder-gray-400 focus:outline-none focus:ring-2 focus:ring-primary focus:border-transparent transition-all"
                  :class="{
                    'border-red-500 focus:ring-red-500': v$.confirmPassword.$error,
                    'border-green-500 focus:ring-green-500': !v$.confirmPassword.$invalid && v$.confirmPassword.$dirty,
                  }"
                  @blur="v$.confirmPassword.$touch"
                  :placeholder="$t('confirm_your_password')"
                >
                <button
                  type="button"
                  class="absolute right-3 top-1/2 -translate-y-1/2 text-gray-500 hover:text-gray-700 dark:text-gray-400 dark:hover:text-gray-200 transition-colors"
                  @click="passwordVisible = !passwordVisible"
                >
                  <i class="fas" :class="passwordVisible ? 'fa-eye-slash' : 'fa-eye'"></i>
                </button>
              </div>
              <span v-if="v$.confirmPassword.$error" class="text-xs text-red-500 mt-1 block">
                {{ v$.confirmPassword.$silentErrors[0].$message }}
              </span>
            </div>
          </div>
        </form>
      </template>

      <template #action>
        <div class="flex gap-3">
          <button
            type="button"
            class="flex-1 px-6 py-3 rounded-lg border border-gray-300 dark:border-gray-600 text-gray-700 dark:text-gray-300 font-medium hover:bg-gray-50 dark:hover:bg-gray-700 transition-colors"
            :disabled="isLoading"
            @click="close"
          >
            {{ $t('cancel') }}
          </button>
          <button
            type="button"
            class="flex-1 px-6 py-3 rounded-lg bg-primary text-white font-medium hover:bg-primary-dark transition-colors flex items-center justify-center gap-2"
            :disabled="isLoading"
            @click="submit"
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
import { required, helpers, sameAs } from '@vuelidate/validators'
import { useVuelidate } from '@vuelidate/core'
import type { ChangePassword, UserListItem } from '~/models/UserModel'
import { useUserStore } from '~/stores/UserStore'

export default defineComponent({
  name: 'UserChangePasswordModal',
  emits: ['item-edited'],
  setup(_, context) {
    const user = reactive({
      id: '',
      password: '',
      confirmPassword: '',
    } as ChangePassword)

    const { t } = useI18n()
    const { changePassword } = useUserStore()

    const passwordVisible = ref(false)
    const isLoading = ref(false)
    const showModal = ref(false)

    const rules = computed(() => ({
      password: {
        required: helpers.withMessage(t('password_field_required'), required),
      },
      confirmPassword: {
        sameAs: helpers.withMessage(t('passwords_need_to_match'), sameAs(user.password)),
      },
    }))

    const v$ = useVuelidate(rules, user)

    const open = (userData: UserListItem) => {
      showModal.value = true
      user.id = userData.id
      user.password = ''
      user.confirmPassword = ''
      passwordVisible.value = false
      v$.value.$reset()
    }

    const close = () => {
      showModal.value = false
      isLoading.value = false
      passwordVisible.value = false
      v$.value.$reset()
      user.id = ''
      user.password = ''
      user.confirmPassword = ''
    }

    const submit = async (e?: MouseEvent) => {
      if (e) e.preventDefault()

      await v$.value.$validate()
      if (v$.value.$error) {
        return
      }

      isLoading.value = true
      try {
        const response = await changePassword(user.id, user.password)
        if (!response) return

        context.emit('item-edited')
        close()
      } finally {
        isLoading.value = false
      }
    }

    return {
      v$,
      user,
      showModal,
      passwordVisible,
      isLoading,
      open,
      close,
      submit,
    }
  },
})
</script>
