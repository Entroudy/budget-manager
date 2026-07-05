<template>
  <div id="topbar" :class="[sidebarOpen ? 'ml-0' : 'xl:ml-256px']"
    class="fixed w-fill top-0 flex h-16 shrink-0 z-50 items-center gap-x-4 dark:bg-brand-darkGray sm:gap-x-6 pr-4 sm:pr-6 xl:pr-8 print:hidden">
    <button type="button" class="btn btn-sm bg-primary text-gray-400 !py-1.5 !px-3 top-0 absolute left-0 h-16 !rounded-none !rounded-br-xl"
      @click="openSidebar">
      <span class="sr-only">{{ $t('open_sidebar') }}</span>
      <span class="cursor-pointer">
        <i class="fas text-white" :class="[sidebarOpen ? 'fa-angle-double-right' : 'fa-angle-double-left']" />
      </span>
    </button>
    <div class="h-6 w-px bg-gray-900/10 xl:hidden" aria-hidden="true" />
  </div>
</template>

<script lang="ts">
import { defineComponent } from 'vue'

export default defineComponent({
  name: 'TheTopbar',
  props: {
    sidebarOpen: {
      type: Boolean,
      required: true
    }
  },
  emits: ['on-open-sidebar'],
  setup(props, { emit }) {
    const { t, locale } = useI18n();


    const showProfileDropdown = ref(false);

    const openSidebar = () => {
      emit('on-open-sidebar');
    };

    const changeLanguage = (lang: string) => {
      locale.value = lang;
    };

    const handleDropdownClick = () => {
      if (showProfileDropdown.value) {
        showProfileDropdown.value = false;
      }
    };

    return {
      showProfileDropdown,
      openSidebar,
      changeLanguage,
      handleDropdownClick
    };
  }
})
</script>

<style>
#topbar {
  transition: margin-left 0.25s ease;
}
</style>
