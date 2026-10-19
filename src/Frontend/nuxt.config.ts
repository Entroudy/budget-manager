// https://nuxt.com/docs/api/configuration/nuxt-config
import { fileURLToPath } from 'node:url'

export default defineNuxtConfig({
  ssr: false,

  experimental: {
    viteEnvironmentApi: true
  },

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
      title: 'Budget Manager – A te pénzügyi kerted',
      meta: [
        { name: 'description', content: 'Közös családi költségvetés, átláthatóan. Kövesd a kiadásokat és bevételeket egy helyen.' },
        { property: 'og:description', content: 'Közös családi költségvetés, átláthatóan. Kövesd a kiadásokat és bevételeket egy helyen.' },
        { name: 'twitter:description', content: 'Közös családi költségvetés, átláthatóan. Kövesd a kiadásokat és bevételeket egy helyen.' }
      ],
      link: [
        { rel: 'icon', type: 'image/png', href: '/favicon.png' },
      ]
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
    vueI18n: fileURLToPath(new URL('./i18n.config.mjs', import.meta.url))
  },

  postcss: {
    plugins: {
      tailwindcss: {},
      autoprefixer: {}
    }
  },

  compatibilityDate: '2024-03-15'
})
