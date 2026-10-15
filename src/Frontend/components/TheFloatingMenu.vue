<template>
  <div class="print:hidden">
    <aside class="fixed inset-y-0 left-0 z-40 hidden w-72 flex-col bg-[#17382d] px-5 py-7 text-[#f6f3e9] shadow-2xl lg:flex">
      <NuxtLink to="/" class="mb-12 flex items-center gap-3 px-3">
        <img src="~/assets/images/budget-manager-logo.png" alt="Budget Manager" class="h-14 w-14 rounded-2xl object-cover shadow-md" />
        <span><span class="block font-display text-xl font-semibold tracking-tight">Budget Manager</span><span class="text-xs text-[#c5d2be]">{{ $t('dashboard') }}</span></span>
      </NuxtLink>
      <p class="mb-3 px-4 text-[10px] font-bold uppercase tracking-[.22em] text-[#9aad9d]">{{ $t('navigation') }}</p>
      <nav class="flex-1">
        <ul class="space-y-2">
          <li v-for="item in menuItems" :key="item.name">
            <NuxtLink :to="item.link" class="group flex items-center gap-4 rounded-xl px-4 py-3.5 text-sm font-semibold transition-all" :class="isActive(item.link) ? 'bg-[#dce6d3] text-[#17382d] shadow-md' : 'text-[#e1e8da] hover:bg-white/10 hover:text-white'">
              <i class="w-5 text-center text-base" :class="item.iconClass" />
              <span>{{ $t(item.name.toLowerCase()) }}</span>
              <PoppyMark v-if="isActive(item.link)" class="ml-auto h-5 w-5" />
            </NuxtLink>
          </li>
        </ul>
      </nav>
      <div class="rounded-2xl border border-white/10 bg-white/5 p-4">
        <p class="font-display text-sm text-[#f4efdf]">{{ $t('small_steps') }}</p>
        <p class="mt-1 text-xs leading-relaxed text-[#b9c8b8]">{{ $t('mindful_money') }}</p>
      </div>
      <button @click="logout" class="mt-5 flex items-center gap-3 rounded-xl px-4 py-3 text-sm font-semibold text-[#e3c9bb] hover:bg-white/10 hover:text-white">
        <i class="fas fa-sign-out-alt w-5 text-center" />{{ $t('logout') }}
      </button>
    </aside>

    <nav class="fixed inset-x-0 top-0 z-50 flex items-center justify-between border-b border-[#dfe2d3] bg-[#fffdf6]/95 px-3 py-2 text-[#17382d] shadow-sm backdrop-blur lg:hidden">
      <NuxtLink to="/" class="flex items-center gap-2 font-display font-semibold text-[#17382d]"><img src="~/assets/images/budget-manager-logo.png" alt="" class="h-9 w-9 rounded-xl object-cover" /> Budget Manager</NuxtLink>
      <div class="flex items-center gap-1">
        <NuxtLink v-for="item in menuItems" :key="item.name" :to="item.link" class="flex h-10 w-10 items-center justify-center rounded-xl" :class="isActive(item.link) ? 'bg-[#24513f] text-white' : 'text-[#526b59] hover:bg-gray-100'" :title="$t(item.name.toLowerCase())"><i :class="item.iconClass" /></NuxtLink>
        <button @click="logout" class="flex h-10 w-10 items-center justify-center rounded-xl text-[#9b5c4d]" :title="$t('logout')"><i class="fas fa-sign-out-alt" /></button>
      </div>
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
      return link === '/' ? this.$route.path === '/' : this.$route.path.startsWith(link)
    },
    ...mapActions(useAuthStore, ['logout']),
  }
})
</script>
