<template>
  <div class="w-full max-w-md mx-auto">
    <div class="space-y-5">
      <div>
        <input
          id="email"
          v-model="form.email"
          type="text"
          placeholder="Email"
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
            :type="passwordVisible ? 'text' : 'password'"
            placeholder="Password"
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

    <div class="mt-5">
      <label class="flex items-center cursor-pointer">
        <input
          id="remember-me"
          v-model="form.rememberMe"
          type="checkbox"
          class="w-4 h-4 rounded border-gray-300 text-gray-900 focus:ring-0 focus:ring-offset-0"
        >
        <span class="ml-2 text-sm text-gray-600 select-none">Remember me</span>
      </label>
    </div>

    <button
      class="w-full mt-6 px-4 py-3 bg-primary text-white rounded-lg hover:bg-primary/90 transition-colors disabled:opacity-50 disabled:cursor-not-allowed flex items-center justify-center"
      @click="loginUser"
      :disabled="isLoading"
    >
      <span>Sign in</span>
      <i v-show="isLoading" class="fas fa-spinner fa-spin ml-2"></i>
    </button>
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
    const form = reactive({
      email: '',
      password: '',
      rememberMe: true
    } as UserLogin)

    const rules = computed(() => {
      return {
        email: {
          required: helpers.withMessage('The email field is required', required)
        },
        password: {
          required: helpers.withMessage('The password field is required', required)
        },
      }
    })
    const v$ = useVuelidate(rules, form)
    const passwordVisible = ref(false)
    const isLoading = ref(false)

    const { login } = useAuthStore()

    const loginUser = async (e: MouseEvent) => {
      isLoading.value = true;
      e.preventDefault()
      v$.value.$validate()
      if(v$.value.$error) isLoading.value = false
      if (!v$.value.$error) {
        isLoading.value = false;
        await login(form)
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
