<template>
  <div class="min-h-screen bg-[#f6f3e9] text-[#253d32] dark:bg-gray-900">
    <TheFloatingMenu />

    <main id="main-content" class="min-h-screen pt-16 lg:pl-72 lg:pt-0">
      <header class="print:hidden flex items-center justify-between gap-4 border-b border-[#e2e1d4] bg-[#fffdf6]/85 px-5 py-5 backdrop-blur md:px-10 md:py-7">
        <div class="flex items-center gap-3">
          <PoppyMark class="h-11 w-11" />
          <div>
            <p class="text-[10px] font-bold uppercase tracking-[.2em] text-[#84917f]">{{ $t('dashboard') }}</p>
            <h1 class="font-display text-2xl font-semibold capitalize text-[#17382d] md:text-3xl">{{ activePageTitle }}</h1>
          </div>
        </div>
        <LanguageSwitcher variant="light" />
      </header>
      <div class="px-3 py-5 md:px-8 md:py-8 xl:px-10">
        <div class="mx-auto w-full max-w-[2000px]">
          <slot />
        </div>
      </div>
    </main>
  </div>
</template>

<script setup lang="ts">
import { useGlobalStore } from '@/stores/global';
import { storeToRefs } from 'pinia';

const globalStore = useGlobalStore();
const { activePageTitleKey } = storeToRefs(globalStore);
const { t } = useI18n();

const activePageTitle = computed(() => {
  return activePageTitleKey.value ? t(activePageTitleKey.value) : '';
});

</script>


<script lang="ts">
import { defineComponent } from 'vue'

export default defineComponent({
  data() {
    return {
      notificationHandler: null as any
    }
  },
  mounted() {
    this.notificationHandler = (data) => {
      this.$showNotification(data.type, data.message)
    }
    this.$listen('notification', this.notificationHandler)
  },
  beforeUnmount() {
    if (this.notificationHandler) {
      this.$unlisten('notification', this.notificationHandler)
    }
  }
})
</script>
