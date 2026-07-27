import { createAuth0 } from '@auth0/auth0-vue'

export default defineNuxtPlugin((nuxtApp) => {
  const domain = useRuntimeConfig().public.auth0Domain
  const clientId = useRuntimeConfig().public.auth0clientId
  const audience = useRuntimeConfig().public.auth0Audience

  const auth0 = createAuth0({
    domain: domain,
    clientId: clientId,
    authorizationParams: {
      redirect_uri: window.location.origin,
      audience: audience
    },
  })

  if (process.client) {
    nuxtApp.vueApp.use(auth0)
  }

  return {
    provide: {
      auth0
    },
  }
})
