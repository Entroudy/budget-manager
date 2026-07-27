import { useAuthStore } from '@/stores/AuthStore'

export default defineNuxtRouteMiddleware(async () => {
  const authStore = useAuthStore()

  if (process.client && !authStore.account.token) {
    try {
      await authStore.getUser()
    } catch {
      // Fall through to the login redirect below.
    }
  }

  if (!authStore.account.token) {
    return navigateTo('/login')
  }
})
