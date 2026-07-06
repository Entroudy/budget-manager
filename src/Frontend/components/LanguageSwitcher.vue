<template>
  <div
    class="inline-flex items-center gap-1 rounded-full border px-1 py-1 backdrop-blur-sm"
    :class="props.variant === 'light'
      ? 'border-gray-200 bg-white text-gray-700'
      : 'border-white/20 bg-white/10 text-white'"
  >
    <button
      v-for="option in options"
      :key="option.value"
      type="button"
      class="flex h-8 min-w-10 items-center justify-center rounded-full px-2 text-xs font-bold uppercase tracking-[0.12em] transition-colors"
      :class="locale === option.value
        ? props.variant === 'light'
          ? 'bg-primary text-white shadow-sm'
          : 'bg-white text-primary shadow-sm'
        : props.variant === 'light'
          ? 'text-gray-500 hover:bg-gray-100'
          : 'text-white/70 hover:bg-white/10'"
      :aria-pressed="locale === option.value"
      :aria-label="option.ariaLabel"
      :title="option.ariaLabel"
      @click="changeLocale(option.value)"
    >
      {{ option.label }}
    </button>
  </div>
</template>

<script setup lang="ts">
import { computed, watch } from 'vue'
import { useAuthStore } from '@/stores/AuthStore'
import { normalizeLocale, persistLocalePreference } from '~/utils/locale'
import type { AppLocale } from '~/utils/locale'

const props = withDefaults(defineProps<{
  variant?: 'header' | 'light'
}>(), {
  variant: 'header'
})

const { t, locale } = useI18n()
const authStore = useAuthStore()

const options = computed(() => [
  { value: 'en' as AppLocale, label: 'EN', ariaLabel: t('english') },
  { value: 'hu' as AppLocale, label: 'HU', ariaLabel: t('hungarian') }
])

const changeLocale = (nextLocale: AppLocale) => {
  locale.value = normalizeLocale(nextLocale)
}

watch(locale, (value) => {
  const normalized = normalizeLocale(value)
  persistLocalePreference(normalized)

  if (authStore.isLoggedIn) {
    authStore.setLocale(normalized)
  }
})
</script>
