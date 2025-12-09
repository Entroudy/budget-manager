import { useAuthStore } from '@/stores/AuthStore'

export default defineNuxtRouteMiddleware(async (to, from) => {
  const { getUser } = useAuthStore()

  try {
    const promise = await getUser()
  } catch (error) {
    if (error) {
      return navigateTo('/login')
    }
  }
})
