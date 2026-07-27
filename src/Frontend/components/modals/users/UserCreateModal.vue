<template>
  <Transition name="modal">
    <BaseModal v-if="showModal" :show-close="true" :showing="showModal" @close="close" :preventBackgroundClose="true">
      <template #title>
        <div class="flex items-center gap-3">
          <div class="w-10 h-10 rounded-full bg-primary/10 flex items-center justify-center">
            <i class="fas fa-user-plus text-primary text-lg"></i>
          </div>
          <div>
            <h2 class="text-xl font-semibold text-gray-900 dark:text-white">{{ $t('create_user') }}</h2>
            <p class="text-sm text-gray-500 dark:text-gray-400">{{ $t('fill_in_user_details') }}</p>
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

            <div class="grid grid-cols-1 md:grid-cols-2 gap-4">
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

            <div>
              <label for="role" class="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-2">
                {{ $t('role') }}
              </label>
              <Multiselect
                id="role"
                v-model="selectedRoleOption"
                v-floating-select="'body'"
                :options="roleOptions"
                :searchable="false"
                :allow-empty="false"
                :show-labels="false"
                :close-on-select="true"
                track-by="value"
                label="label"
                :placeholder="$t('role')"
                class="w-full role-multiselect"
              />
            </div>
          </div>

          <!-- Security Section -->
          <div class="space-y-4 pt-4 border-t border-gray-200 dark:border-gray-700">
            <h3 class="text-sm font-semibold text-gray-700 dark:text-gray-300 uppercase tracking-wide">
              {{ $t('security') }}
            </h3>

            <div class="grid grid-cols-1 md:grid-cols-2 gap-4">
              <!-- Password -->
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
                    @click="passwordVisible = !passwordVisible"
                    class="absolute right-3 top-1/2 -translate-y-1/2 text-gray-500 hover:text-gray-700 dark:text-gray-400 dark:hover:text-gray-200 transition-colors"
                  >
                    <i class="fas" :class="passwordVisible ? 'fa-eye-slash' : 'fa-eye'"></i>
                  </button>
                </div>
                <span v-if="v$.password.$error" class="text-xs text-red-500 mt-1 block">
                  {{ v$.password.$silentErrors[0].$message }}
                </span>
              </div>

              <!-- Confirm Password -->
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
                    @click="passwordVisible = !passwordVisible"
                    class="absolute right-3 top-1/2 -translate-y-1/2 text-gray-500 hover:text-gray-700 dark:text-gray-400 dark:hover:text-gray-200 transition-colors"
                  >
                    <i class="fas" :class="passwordVisible ? 'fa-eye-slash' : 'fa-eye'"></i>
                  </button>
                </div>
                <span v-if="v$.confirmPassword.$error" class="text-xs text-red-500 mt-1 block">
                  {{ v$.confirmPassword.$silentErrors[0].$message }}
                </span>
              </div>
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
            <span>{{ $t('create') }}</span>
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
import { storeToRefs } from 'pinia'
import Multiselect from 'vue-multiselect'
import type { UserCreate } from '~/models/UserModel'
import { useUserStore } from '~/stores/UserStore'
import { DEFAULT_USER_ROLE, USER_ROLES, getUserRoleLabelKey } from '~/core/userRoles'

export default defineComponent({
  name: 'UserCreateModal',
  components: {
    Multiselect
  },
  emits: ['item-created'],
  setup(props, context) {
    const user = reactive({
      lastName: '',
      firstName: '',
      nickName: '',
      email: '',
      password: '',
      confirmPassword: '',
      role: DEFAULT_USER_ROLE,
    } as UserCreate)

    const { t } = useI18n();

    const passwordVisible = ref(false)
    const isLoading = ref(false)
    const showModal = ref(false)

    const userStore = useUserStore()
    const { createUser } = userStore
    const { roles } = storeToRefs(userStore)

    const roleOptions = computed(() =>
      (roles.value.length > 0 ? roles.value : USER_ROLES).map(role => ({
        value: role,
        label: t(getUserRoleLabelKey(role)),
      }))
    )
    const selectedRoleOption = computed({
      get: () => roleOptions.value.find(option => option.value === user.role) ?? roleOptions.value[0] ?? null,
      set: (option: { value: string } | null) => {
        user.role = option?.value || DEFAULT_USER_ROLE
      }
    })

    const rules = computed(() => {
      return {
        lastName: {
          required: helpers.withMessage(t('last_name_field_required'), required)
        },
        firstName: {
          required: helpers.withMessage(t('first_name_field_required'), required)
        },
        nickName: {
          required: helpers.withMessage(t('nickname_field_required'), required)
        },
        email: {
          required: helpers.withMessage(t('email_field_required'), required)
        },
        password: {
          required: helpers.withMessage(t('password_field_required'), required)
        },
        confirmPassword: {
          sameAs: helpers.withMessage(t('passwords_need_to_match'), sameAs(user.password))
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
      user.role = DEFAULT_USER_ROLE
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
      roleOptions,
      selectedRoleOption,
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
