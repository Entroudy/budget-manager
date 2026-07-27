import mitt from 'mitt'

interface Notification {
  type: string,
  message: string
}

type ApplicationEvents = {
  'notification': Notification,
};

export default defineNuxtPlugin(() => {
  const emitter = mitt<ApplicationEvents>()

  return {
    provide: {
      event: emitter.emit,
      listen: emitter.on,
      unlisten: emitter.off
    }
  }
})
