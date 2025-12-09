export default defineNuxtPlugin((nuxtApp) => {
  const wrapperClass = '.multiselect__content-wrapper'

  const calculateBase = (el) => {
    const wrapperElement = el.querySelector(wrapperClass)

    wrapperElement.style.width = `${el.clientWidth}px`
    wrapperElement.style.position = 'fixed'
    wrapperElement.style.bottom = 'auto'
  }
  const calculateWrapperPosition = (el) => {
    const { top, left, height } = el.getBoundingClientRect()
    const wrapperElement = el.querySelector(wrapperClass)
    const wrapperHeight = wrapperElement.getBoundingClientRect().height
    const isAbove = el.classList.contains('multiselect--above')

    const newTop = isAbove ? top + 2 - wrapperHeight : top + height

    if (newTop === 0) {
      return
    }

    wrapperElement.style.top = `${newTop}px`
    wrapperElement.style.left = `${Math.abs(left)}px`;
  }

  const directive = {
    beforeMount(el, binding) {
      const parentElement = document.querySelector(binding.value)

      const calculatePositionCallback = () => calculateWrapperPosition(el)

      calculatePositionCallback()

      // Store the callback function on the element itself.
      el._calculatePositionCallback = calculatePositionCallback

      if (parentElement) {
        parentElement.addEventListener('scroll', calculatePositionCallback)

        // Create a new MutationObserver that recalculates the position when the children of parentElement are modified.
        const observer = new MutationObserver(calculatePositionCallback)
        // Start observing parentElement for childList changes.
        observer.observe(parentElement, { childList: true })

        // Store the MutationObserver on the element itself, so we can disconnect it later.
        el._observer = observer
      }

      document.addEventListener('scroll', calculatePositionCallback)
    },
    mounted(el) {
      calculateBase(el)
    },
    updated(el) {
      if (!el?.classList?.contains('multiselect--active')) {
        return
      }

      el?._calculatePositionCallback()
    },
    beforeUnmount(el, binding) {
      const parentElement = document.querySelector(binding.value)

      // Disconnect the MutationObserver.
      el._observer?.disconnect()

      parentElement?.removeEventListener('scroll', el._calculatePositionCallback)
      document.removeEventListener('scroll', el._calculatePositionCallback)

      // clean up styles on unmount
      const wrapperElement = el.querySelector(wrapperClass)
      wrapperElement.style.width = ''
      wrapperElement.style.position = ''
      wrapperElement.style.top = ''
      wrapperElement.style.bottom = ''
    },
  }

  nuxtApp.vueApp.directive('floating-select', directive)
  /*
    Use:

    <Multiselect v-floating-select ...

    Optionally with an element:

    <Multiselect v-floating-select="body" ...

    or

    <Multiselect v-floating-select=".a-parent" ...
  */
})
