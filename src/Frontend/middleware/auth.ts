import { getUser } from '~/services/api/JwtService'

export default defineNuxtRouteMiddleware(async (to, from) => {
  try {
    const user = getUser()
    if (!user) {
      return navigateTo('/login')
    }
  } catch (error) {
    if (error) {
      return navigateTo('/login')
    }
  }
})
