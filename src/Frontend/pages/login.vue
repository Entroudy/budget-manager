<template>
  <div class="w-full max-w-md mx-auto">
    <div class="space-y-5">
      <div>
        <input
          id="email"
          v-model="form.email"
          type="email"
          autocomplete="username"
          :placeholder="$t('email_placeholder')"
          class="w-full px-4 py-3 border border-gray-200 rounded-lg focus:outline-none focus:border-gray-400 transition-colors"
          :class="{
            'border-red-500 focus:border-red-500': v$.email.$error,
          }"
          @change="v$.email.$touch"
        >
        <span v-if="v$.email.$error" class="text-xs text-red-500 mt-1 block">{{ v$.email.$silentErrors[0].$message }}</span>
      </div>

      <div>
        <div class="relative">
          <input
            id="password"
            v-model="form.password"
            autocomplete="current-password"
            :type="passwordVisible ? 'text' : 'password'"
            :placeholder="$t('password_placeholder')"
            class="w-full px-4 py-3 pr-12 border border-gray-200 rounded-lg focus:outline-none focus:border-gray-400 transition-colors"
            :class="{
              'border-red-500 focus:border-red-500': v$.password.$error,
            }"
            @change="v$.password.$touch"
          >
          <button
            type="button"
            class="absolute right-4 top-1/2 -translate-y-1/2 text-gray-400 hover:text-gray-600"
            @click="passwordVisible = !passwordVisible"
          >
            <i class="fas text-sm" :class="[passwordVisible ? 'fa-eye-slash' : 'fa-eye']" />
          </button>
        </div>
        <span v-if="v$.password.$error" class="text-xs text-red-500 mt-1 block">{{ v$.password.$silentErrors[0].$message }}</span>
      </div>
    </div>

    <div class="mt-6 flex items-center justify-between gap-3">
      <label for="remember-me" class="!mb-0 !inline-flex !items-center !gap-2 cursor-pointer text-xs font-medium !leading-none text-[#d5dfd0]">
        <input
          id="remember-me"
          v-model="form.rememberMe"
          type="checkbox"
          class="!m-0 !block !h-4 !w-4 shrink-0 rounded border-white/30 !bg-[#8b9b7b]"
        >
        {{ $t('remember_me') }}
      </label>
      <button
        class="flex min-w-36 items-center justify-center gap-2 rounded-xl bg-[#dce6d3] px-5 py-3 font-semibold text-[#17382d] shadow-lg hover:bg-white disabled:opacity-50"
        @click="loginUser"
        :disabled="isLoading"
      >
        <span>{{ $t('sign_in') }}</span>
        <i v-show="isLoading" class="fas fa-spinner fa-spin ml-2"></i>
      </button>
    </div>
  </div>
</template>

<script lang="ts">
import { defineComponent } from 'vue'
import { useAuthStore } from '@/stores/AuthStore'
import type { UserLogin } from '@/models/UserModel'
import { required, helpers } from '@vuelidate/validators'
import { useVuelidate } from '@vuelidate/core'

definePageMeta({ layout: 'auth' })

export default defineComponent({
  name: 'LoginPage',
  setup(props) {
    const { t } = useI18n()

    const form = reactive({
      email: '',
      password: '',
      rememberMe: true
    } as UserLogin)

    const rules = computed(() => {
      return {
        email: {
          required: helpers.withMessage(t('email_field_required'), required)
        },
        password: {
          required: helpers.withMessage(t('password_field_required'), required)
        },
      }
    })
    const v$ = useVuelidate(rules, form)
    const passwordVisible = ref(false)
    const isLoading = ref(false)

    const { login } = useAuthStore()

    const loginUser = async (e: MouseEvent) => {
      e.preventDefault()
      if (!(await v$.value.$validate())) return
      isLoading.value = true
      try {
        await login(form)
      } finally {
        isLoading.value = false
      }
    }
    return {
      form,
      loginUser,
      passwordVisible,
      isLoading,
      v$
    }
  }
})
</script>
