// https://nuxt.com/docs/api/configuration/nuxt-config
export default defineNuxtConfig({
  ssr: false,

  runtimeConfig: {
    public: {
      apiUrl: process.env.NUXT_API_URL,
      auth0Domain: process.env.NUXT_AUTH0_DOMAIN,
      auth0clientId: process.env.NUXT_AUTH0_CLIENT_ID,
      auth0Audience: process.env.NUXT_AUTH0_AUDIENCE
    }
  },

  app: {
    head: {
      title: 'Budget Manager',
      link: []
    }
  },

  devtools: {
    enabled: true
  },

  css: ['~/assets/css/main.css', 'vue-toast-notification/dist/theme-default.css'],

  imports: {
    dirs: ['./stores']
  },

  typescript: {
    typeCheck: false
  },

  components: [
    {
      path: '~/components',
      pathPrefix: false
    }
  ],

  modules: ["@nuxtjs/i18n"],

  i18n: {
    strategy: 'no_prefix',
    detectBrowserLanguage: false,
    locales: [
      'hu',
      'en'
     ],
    defaultLocale: 'en',
    vueI18n: './i18n.config.mjs'
  },

  postcss: {
    plugins: {
      tailwindcss: {},
      autoprefixer: {}
    }
  },

  compatibilityDate: '2024-03-15'
})
