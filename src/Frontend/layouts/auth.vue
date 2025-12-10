<template>
  <div class="flex min-h-screen items-center justify-center p-4 bg-white">
    <div class="w-full max-w-md">
      <!-- Simple Financial Icon -->
      <div class="text-center mb-8">
        <div class="inline-flex items-center justify-center w-16 h-16 rounded-2xl mb-4 bg-primary">
          <i class="fas fa-wallet text-white text-2xl"></i>
        </div>
        <h1 class="text-2xl font-semibold text-primary mb-1">Budget Manager</h1>
        <p class="text-sm text-gray-600">Sign in to manage your finances</p>
      </div>

      <!-- Login Card -->
      <div class="bg-gray-50 rounded-2xl shadow-sm border border-gray-200 p-8">
        <slot />
      </div>
    </div>
  </div>
</template>

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
