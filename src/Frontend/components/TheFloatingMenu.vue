<template>
  <div class="fixed top-4 left-1/2 -translate-x-1/2 z-50 print:hidden">
    <nav class="bg-white/95 backdrop-blur-sm rounded-lg shadow-lg px-6 py-3 border border-gray-200">
      <ul class="flex items-center gap-6">
        <li v-for="item in menuItems" :key="item.name">
          <NuxtLink
            :to="item.link"
            class="group flex items-center justify-center w-12 h-12 rounded-lg transition-all duration-200"
            :class="[
              isActive(item.link)
                ? 'bg-primary text-white'
                : 'text-gray-600 hover:text-primary hover:bg-gray-100'
            ]"
            :title="$t(item.name.toLowerCase())"
          >
            <i class="text-xl" :class="item.iconClass" />
          </NuxtLink>
        </li>
        <li class="ml-2 pl-2 border-l border-gray-300">
          <button
            @click="logout"
            class="group flex items-center justify-center w-12 h-12 rounded-lg text-gray-600 hover:text-red-600 hover:bg-red-50 transition-all duration-200"
            :title="$t('logout')"
          >
            <i class="text-xl fas fa-sign-out-alt" />
          </button>
        </li>
      </ul>
    </nav>
  </div>
</template>

<script lang="ts">
import { defineComponent } from 'vue'
import { mapActions } from 'pinia'
import { TopMenu } from '@/core/menuItems'

export default defineComponent({
  name: 'TheFloatingMenu',
  data() {
    return {
      menuItems: TopMenu
    }
  },
  methods: {
    isActive(link: string) {
      return this.$route.path.startsWith(link) && link !== '/'
    },
    ...mapActions(useAuthStore, ['logout']),
  }
})
</script>
