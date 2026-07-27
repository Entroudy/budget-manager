<template>
  <div class="min-h-screen bg-gradient-to-br from-gray-50 via-gray-100 to-gray-50 dark:from-gray-900 dark:via-gray-800 dark:to-gray-900">
    <TheFloatingMenu />

    <main id="main-content" class="relative">
      <!-- Modern Header Section -->
      <div class="print:hidden bg-gradient-to-r from-primary via-primary-dark to-primary overflow-hidden pb-24 md:pb-8">
        <!-- Decorative Background Elements -->
        <div class="absolute">
          <div class="absolute -top-10 -right-10 w-48 h-48 md:w-64 md:h-64 bg-white/10 rounded-full blur-2xl"></div>
          <div class="absolute top-1/2 -left-10 w-48 h-48 md:w-64 md:h-64 bg-white/10 rounded-full blur-2xl"></div>
        </div>

        <!-- Header Content -->
        <div class="relative px-6 md:px-12 py-12 md:py-16">
          <div class="max-w-7xl mx-auto">
            <div class="flex items-center gap-4 mb-4">
              <div class="w-12 h-12 rounded-xl bg-white/20 backdrop-blur-sm flex items-center justify-center">
                <i class="fas fa-layer-group text-white text-xl"></i>
              </div>
              <div class="flex-1 min-w-0">
                <p class="text-white/80 text-sm font-medium uppercase tracking-wider">{{ $t('dashboard') }}</p>
                <h1 class="text-white font-bold text-3xl md:text-4xl capitalize">
                  {{ activePageTitle }}
                </h1>
              </div>
              <div class="shrink-0">
                <LanguageSwitcher />
              </div>
            </div>

            <!-- Breadcrumb -->
            <div class="flex items-center gap-2 text-white/70 text-sm mt-4">
              <i class="fas fa-home text-xs"></i>
              <span>/</span>
              <span class="capitalize">{{ activePageTitle }}</span>
            </div>
          </div>
        </div>
      </div>

      <!-- Main Content Area -->
      <div class="screen:px-4 screen:py-6 screen:md:px-24 screen:md:py-12 screen:-mt-16 screen:md:-mt-24">
        <div class="w-full mx-auto">
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
