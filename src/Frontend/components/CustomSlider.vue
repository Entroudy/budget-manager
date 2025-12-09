<template>
  <div :class="[isOpen ? 'fixed inset-0 overflow-hidden bg-black/70 z-50' : '']">
    <div :class="[isOpen ? 'absolute inset-0 overflow-hidden' : '']">
      <div class="panel-wrap w-full md:w-1/2 lg:w-1/3 shadow-xl bg-white" :class="[isOpen ? 'open' : '']">
        <div class="panel">
          <div class="flex h-full flex-col w-full">
            <div class="panel-header">
              <div class="flex w-full justify-between my-2">
                <div class="flex items-center">
                  <div class="mr-4 flex h-7 items-center">
                    <button type="button" class="relative -m-2 p-2 text-gray-400 hover:text-gray-500" @click="close">
                      <span class="absolute -inset-0.5"></span>
                      <span class="sr-only">Close panel</span>
                      <svg class="h-6 w-6" fill="none" viewBox="0 0 24 24" stroke-width="1.5" stroke="currentColor"
                        aria-hidden="true">
                        <path stroke-linecap="round" stroke-linejoin="round" d="M6 18L18 6M6 6l12 12"></path>
                      </svg>
                    </button>
                  </div>
                  <h2 class="text-xl font-medium text-gray-900" id="slide-over-title">{{ title }}</h2>
                </div>
                <div class="space-x-4">

                  <slot name="header"></slot>
                </div>
              </div>
            </div>
            <div class="panel-body">
              <slot name="body"></slot>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import { storeToRefs } from 'pinia'

export default defineComponent({
  name: 'CustomSlider',
  props: {
    isOpen: {
      required: true,
      type: Boolean
    },
    title: {
      required: false,
      type: String
    }
  },
  emits: ['slider-opened', 'slider-closed'],
  setup(props, context) {
    onMounted(() => {
      document.addEventListener('keydown', (e) => {
        if (e.keyCode == 27) {
          close()
        }
      })
    })

    /* const isOpen = ref(false) */

    const { setOverlay } = useLayoutStore()
    const { isOverlayVisible } = storeToRefs(useLayoutStore())

    /* watch(isOverlayVisible, (newValue) => {
      setTimeout(() => {
        isOpen.value = newValue
        if (newValue) {
          context.emit('slider-opened')
        }
      }, 50);
    }); */

    /* watch(() => props.isOpen, (newValue) => {
      if (newValue) {
        context.emit('slider-opened')
      }
    }) */

    const close = () => {
      context.emit('slider-closed')
      /* setOverlay(false) */
    }


    return {
      open,
      close,
    }
  }
})
</script>

<style scoped>
.panel-wrap {
  z-index: 9999;
  position: fixed;
  top: 0;
  bottom: 0;
  right: 0;
  transform: translateX(100%);
  transition: transform 0.25s ease;
}

.panel {
  z-index: 9999;
  position: absolute;
  top: 0;
  bottom: 0;
  left: 0;
  right: 0;
}

.panel-wrap.open {
  transform: translateX(0%);
}

.panel-header {
  display: flex;
  flex: 0;
  align-items: center;
  padding: 16px 24px;
  font-size: 16px;
  line-height: 1.5;
  border-bottom: 1px solid rgba(5, 5, 5, 0.06);
  box-sizing: border-box;
}

.panel-body {
  flex: 1;
  min-width: 0;
  min-height: 0;
  padding: 24px;
  overflow: auto;
  box-sizing: border-box;
}
</style>
