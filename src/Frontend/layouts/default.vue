<template>
  <div>
    <TheFloatingMenu />

    <main id="main-content" class="relative bg-primary">
      <div class="relative print:hidden h-96 flex items-center bg-primary border-b border-gray-200">
        <span class="left-12 md:left-8 text-white font-bold text-2xl md:text-3xl capitalize px-12 md:px-8">
          {{ activePageTitle }}
        </span>
      </div>
      <div class="screen:px-4 screen:py-6 screen:md:px-8 screen:md:py-10 screen:absolute screen:top-20 screen:md:top-1/2 screen:w-full">
        <slot />
      </div>
    </main>
  </div>
</template>

<script setup lang="ts">
import { useGlobalStore } from '@/stores/global';
import { storeToRefs } from 'pinia';

const globalStore = useGlobalStore();
const { activePageTitle } = storeToRefs(globalStore);

</script>


<script lang="ts">
import { defineComponent } from 'vue'

export default defineComponent({
  mounted() {
    this.$listen('notification', (data) => {
      this.$showNotification(data.type, data.message)
    })
  }
})
</script>
