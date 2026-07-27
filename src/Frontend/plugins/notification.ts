import { useToast, ToastPosition } from 'vue-toast-notification'
const toast = useToast()

export default defineNuxtPlugin(() => {
  function getMessageDuration(msg) {
    const length = msg.length
    let duration

    switch (
      true
    ) {
      case length === 0:
        duration = 0
        break
      case length <= 100:
        duration = 6500
        break
      case length === 150:
        duration = 15000
        break
      case length > 100:
        duration = 10000
        break
      default:
        duration = 6500
    }

    return duration
  }

  return {
    provide: {
      showNotification: (type: string, message: string, position = 'top-right' as ToastPosition) => {
        if (/mobile|tablet/i.test(navigator.userAgent)) {
          position = 'top'
        }

        let iconClass
        let iconColor = 'text-white'

        switch (type) {
          case 'success':
            iconClass = 'far fa-check'
            break
          case 'error':
            iconClass = 'fas fa-times'
            break
          case 'warning':
            iconClass = 'fal fa-exclamation-triangle'
            iconColor = 'text-black'
            break
          case 'info':
            iconClass = 'fas fa-exclamation'
            break
          default:
            break
        }
        const duration = getMessageDuration(message)

        toast.open({
          message: `<span class="capitalize"><i class="${iconClass} ${iconColor} mr-2"></i> ${message}</span>`,
          type,
          position,
          duration: duration,
        })
      },
    },
  }
})
