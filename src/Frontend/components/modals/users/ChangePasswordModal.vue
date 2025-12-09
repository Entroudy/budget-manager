<template>
    <Transition name="modal">
      <BaseModal v-if="showModal" :show-close="true" :showing="showModal" @close="close" :preventBackgroundClose="true">
        <template #title>
          <span class="break-normal">
            {{ $t('change_password') }}
          </span>
        </template>
        <template #body>
          <form v-enter-submit="submit">
  
            <div class="grid grid-cols-1 gap-x-6 gap-y-8 sm:grid-cols-6">

              <div class="sm:col-span-3">
                <label for="password" class="capitalize-block">{{ $t('password') }}</label>
                <div class="flex items-center">
                  <input id="password" v-model="user.password" :type="passwordVisible ? 'text' : 'password'"
                    class="w-full">
                    <i class="fas text-black px-4 text-sm cursor-pointer"
                    :class="[passwordVisible ? 'fa-lock-open' : 'fa-lock']" @click="passwordVisible = !passwordVisible" />
                </div>
              </div>

              <div class="sm:col-span-3">
                <label for="confirmPassword" class="required capitalize-block">{{ $t('confirm_password') }}</label>
                <div class="flex items-center">
                  <input id="confirmPassword" v-model="user.confirmPassword" :type="passwordVisible ? 'text' : 'password'"
                    class="w-full" :class="{
                      '!border-red-600 !focus-visible:border-red-600': v$.confirmPassword.$error,
                      '!border-green-500 ': !v$.confirmPassword.$invalid,
                    }" @change="v$.confirmPassword.$touch">
                    <i class="fas text-black px-4 text-sm cursor-pointer"
                    :class="[passwordVisible ? 'fa-lock-open' : 'fa-lock']" @click="passwordVisible = !passwordVisible" />
                </div>
                <span v-if="v$.confirmPassword.$error" class="text-xs text-red-600">{{ v$.confirmPassword.$silentErrors[0].$message
                  }}</span>
              </div>
  
            </div>
            
          </form>
        </template>
        <template #action>
          <button class="btn bg-primary w-full text-white capitalize" @click="submit" :disabled="isLoading">
            {{ $t('save') }}<i v-show="isLoading" class="fas fa-spinner fa-spin text-white ml-2"></i>
          </button>
        </template>
      </BaseModal>
    </Transition>
  </template>
  
  <script lang="ts">
  import { defineComponent } from 'vue'
  import { requiredIf, required, helpers, sameAs } from '@vuelidate/validators'
  import { useVuelidate } from '@vuelidate/core'
  import type { UserUpdate, UserListItem, ChangePassword } from '~/models/UserModel'
  import { useUserStore } from '~/stores/UserStore'
  
  export default defineComponent({
    name: 'UserCreateModal',
    emits: ['item-created'],
    setup(props, context) {
      const user = reactive({
        id: '',
        password: '',
        confirmPassword: '',
      } as ChangePassword)
  
      const { t } = useI18n();
  
      const passwordVisible = ref(false)
      const isLoading = ref(false)
      const showModal = ref(false)
  
      const { changePassword } = useUserStore()
  
      const rules = computed(() => {
        return {
          password: {
            required: helpers.withMessage('The password field is required', required)
          },
          confirmPassword: {
            sameAs: helpers.withMessage('Password`s need to match', sameAs(user.password))
          },
        }
      })
      const v$ = useVuelidate(rules, user)
  
      const open = (userData: UserListItem) => {
        showModal.value = true
        Object.assign(user, userData);
      }
  
      const close = () => {
        showModal.value = false
        v$.value.$reset()
      }

      const submit = async (e: MouseEvent) => {
        if (e) e.preventDefault()
        isLoading.value = true
        v$.value.$validate()
        if (v$.value.$error) isLoading.value = false
        if (!v$.value.$error) {
          const response = await changePassword(user.id, user)
          isLoading.value = false
          if (!response) { return }
          context.emit('item-created')
          v$.value.$reset()
          close()
        }
      }
  
      return {
        v$,
        rules,
        user,
        showModal,
        passwordVisible,
        open,
        close,
        submit,
        isLoading
      }
    }
  })
  </script>
  