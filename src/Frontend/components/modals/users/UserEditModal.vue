<template>
    <Transition name="modal">
      <BaseModal v-if="showModal" :show-close="true" :showing="showModal" @close="close" :preventBackgroundClose="true">
        <template #title>
          <span class="break-normal">
            {{ $t('update') }}
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
              <span v-if="v$.firstName.$error" class="text-xs text-red-600">{{ v$.firstName.$silentErrors[0].$message
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
  import type { UserUpdate, UserListItem } from '~/models/UserModel'
  import { useUserStore } from '~/stores/UserStore'
  
  export default defineComponent({
    name: 'UserCreateModal',
    emits: ['item-created'],
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
        open,
        close,
        submit,
        isLoading
      }
    }
  })
  </script>
  