<template>
  <div v-if="showing" class="fixed inset-0 z-50 h-full">
    <div
      class="fixed inset-0 z-50 flex h-full items-center justify-center bg-[#17382d]/55 backdrop-blur-sm md:fixed md:h-auto md:p-6"
      @click.self="preventOutsideClick">
      <div
        class="flex h-full max-h-full w-full flex-col bg-white dark:bg-brand-darkGray shadow-2xl md:h-auto md:max-h-[92vh] md:rounded-3xl"
        :class="[maxWidth, textColor]">
        <!-- Header - Fixed -->
        <div class="flex-shrink-0 p-4 md:p-6 border-b border-gray-200 dark:border-gray-700">
          <div class="flex items-center justify-between space-x-3">
            <div class="flex items-center space-x-3">
              <slot v-if="$slots.icon" name="icon" />
              <h3 v-if="$slots.title" class="font-display text-xl font-semibold text-[#234434] dark:text-white flex">
                <slot name="title" />
              </h3>
            </div>
            <button v-if="showClose" class="btn-icon flex cursor-pointer" aria-label="close" role="button"
              @click.prevent="close">
              <i class="fas fa-times text-2xl text-gray-800 dark:text-white" />
            </button>
          </div>
        </div>

        <!-- Body - Scrollable -->
        <div v-if="$slots.body" class="flex-1 overflow-y-auto p-4 md:p-6">
          <slot name="body" />
        </div>

        <!-- Footer - Fixed -->
        <div v-if="$slots.action" class="flex-shrink-0 p-4 md:p-6 border-t border-gray-200 dark:border-gray-700">
          <slot name="action" />
        </div>
      </div>
    </div>
  </div>
</template>

<script lang="ts">
import { defineComponent } from 'vue'

export default defineComponent({
  name: 'BaseModal',
  components: {

  },
  props: {
    showing: {
      required: true,
      type: Boolean
    },
    showClose: {
      type: Boolean,
      default: true
    },
    preventBackgroundClose: {
      type: Boolean,
      default: false
    },
    maxWidth: {
      type: String,
      default: 'md:max-w-4xl'
    },
    textColor: {
      type: String,
      default: 'text-black'
    }
  },
  emits: ['close'],
  mounted: function () {
    if (this.showClose) {
      document.addEventListener('keydown', (e) => {
        if (e.keyCode == 27) {
          this.close()
        }
      })
    }
  },
  beforeUnmount: function () {
    this.close()
  },
  methods: {
    close() {
      this.$emit('close')
    },
    preventOutsideClick() {
      if (!this.preventBackgroundClose) {
        this.close()
      }
    }
  }
})
</script>
