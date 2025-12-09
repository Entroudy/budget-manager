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

            <div class="sm:col-span-3">
              <label for="lastName" class="required capitalize-block">{{ $t('last_name') }}</label>
              <input id="lastName" v-model="user.lastName" type="text" class="w-full" :class="{
                '!border-red-600 !focus-visible:border-red-600': v$.lastName.$error,
                '!border-green-500 ': !v$.lastName.$invalid,
              }" @change="v$.lastName.$touch">
              <span v-if="v$.lastName.$error" class="text-xs text-red-600">{{ v$.lastName.$silentErrors[0].$message
                }}</span>
            </div>

            <div class="sm:col-span-3">
              <label for="firstName" class="required capitalize-block">{{ $t('first_name') }}</label>
              <input id="firstName" v-model="user.firstName" type="text" class="w-full" :class="{
                '!border-red-600 !focus-visible:border-red-600': v$.firstName.$error,
                '!border-green-500 ': !v$.firstName.$invalid,
              }" @change="v$.firstName.$touch">
              <span v-if="v$.firstName.$error" class="text-xs text-red-600">{{ v$.fistName.$silentErrors[0].$message
                }}</span>
            </div>

            <div class="sm:col-span-3">
              <label for="email" class="required capitalize-block">{{ $t('email') }}</label>
              <input id="email" v-model="user.email" type="text" class="w-full" :class="{
                '!border-red-600 !focus-visible:border-red-600': v$.email.$error,
                '!border-green-500 ': !v$.email.$invalid,
              }" @change="v$.email.$touch">
              <span v-if="v$.email.$error" class="text-xs text-red-600">{{ v$.email.$silentErrors[0].$message
                }}</span>
            </div>

            <div class="sm:col-span-3">
              <label for="nickName" class="required capitalize-block">{{ $t('nickName') }}</label>
              <input id="nickName" v-model="user.nickName" type="text" class="w-full" :class="{
                '!border-red-600 !focus-visible:border-red-600': v$.nickName.$error,
                '!border-green-500 ': !v$.nickName.$invalid,
              }" @change="v$.nickName.$touch">
              <span v-if="v$.nickName.$error" class="text-xs text-red-600">{{ v$.nickName.$silentErrors[0].$message
                }}</span>
            </div>

            <div class="sm:col-span-3">
              <label for="password" class="required capitalize-block">{{ $t('password') }}</label>
              <div class="flex items-center">
                <input id="password" v-model="user.password" :type="passwordVisible ? 'text' : 'password'"
                  class="w-full" :class="{
                    '!border-red-600 !focus-visible:border-red-600': v$.password.$error,
                    '!border-green-500 ': !v$.password.$invalid,
                  }" @change="v$.password.$touch">
                  <i class="fas fa-eye text-black px-4 text-sm cursor-pointer"
                  :class="[passwordVisible ? 'fa-lock-open' : 'fa-lock']" @click="passwordVisible = !passwordVisible" />
              </div>
              <span v-if="v$.password.$error" class="text-xs text-red-600">{{ v$.password.$silentErrors[0].$message
                }}</span>
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
          {{ $t('create') }}<i v-show="isLoading" class="fas fa-spinner fa-spin text-white ml-2"></i>
        </button>
      </template>
    </BaseModal>
  </Transition>
</template>

<script lang="ts">
import { defineComponent } from 'vue'
import { required, helpers, sameAs } from '@vuelidate/validators'
import { useVuelidate } from '@vuelidate/core'
import type { UserCreate } from '~/models/UserModel'
import { useUserStore } from '~/stores/UserStore'

export default defineComponent({
  name: 'UserCreateModal',
  emits: ['item-created'],
  setup(props, context) {
    const user = reactive({
      lastName: '',
      firstName: '',
      nickName: '',
      email: '',
      password: '',
      confirmPassword: '',
    } as UserCreate)

    const { t } = useI18n();

    const passwordVisible = ref(false)
    const isLoading = ref(false)
    const showModal = ref(false)

    const { createUser } = useUserStore()

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
        },
        password: {
          required: helpers.withMessage('The password field is required', required)
        },
        confirmPassword: {
          sameAs: helpers.withMessage('Password`s need to match', sameAs(user.password))
        },
      }
    })
    const v$ = useVuelidate(rules, user)

    const open = () => {
      showModal.value = true
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
      user.password = ''
      user.confirmPassword = ''
    }

    const submit = async (e: MouseEvent) => {
      if (e) e.preventDefault()
      isLoading.value = true
      v$.value.$validate()
      if (v$.value.$error) isLoading.value = false
      if (!v$.value.$error) {
        const response = await createUser(user)
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
