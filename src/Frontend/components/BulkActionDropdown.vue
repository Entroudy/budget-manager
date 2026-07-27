<template>
  <div class="relative z-50">

    <div class="flex justify-center items-center">

      <slot name="text"></slot>
      <div class="cursor-pointer" v-click-outside="handleDropdownClick" @click="toggleDropdown">
        <slot name="trigger"></slot>
      </div>
    </div>


    <div :class="[showBulkActionDropdown ? '' : 'hidden']"
      class="absolute right-1/2 translate-x-1/2 z-10 mt-2 w-32 origin-top-right cursor-pointer rounded-md bg-white dark:bg-brand-darkGray shadow-lg ring-1 ring-black ring-opacity-5 focus:outline-none"
      role="menu" aria-orientation="vertical" aria-labelledby="menu-button" tabindex="-1">
      <div class="py-1" role="none">
        <span v-for="action in actions" :key="action.label" @click="() => action.method(itemId)"
          class="capitalize-block hover:bg-brand-darkGray/80 hover:text-white block rounded-md px-4 py-2 text-sm dark:text-white"
          role="menuitem" tabindex="-1">
          {{ $t(action.label) }}
        </span>
      </div>
    </div>
  </div>
</template>

<script>
import { defineComponent } from 'vue'

export default defineComponent({
  name: 'BulkActionDropdown',
  props: {
    actions: { type: Array, required: true },
    itemId: { type: [String, Number], required: true }
  },
  setup(props) {
    const showBulkActionDropdown = ref(false);

    const toggleDropdown = () => {
      showBulkActionDropdown.value = !showBulkActionDropdown.value;
    };

    const handleDropdownClick = () => {
      showBulkActionDropdown.value = false;
    };

    return {
      showBulkActionDropdown,
      toggleDropdown,
      handleDropdownClick,
      props
    };
  }
})
</script>

<style scoped>
/* Your styles */
</style>
