export default defineNuxtPlugin((nuxtApp) => {
  const prefersDarkMode = false
  /* const prefersDarkMode = window.matchMedia && window.matchMedia('(prefers-color-scheme: dark)').matches; */

  if (prefersDarkMode) {
    document.documentElement.classList.add('dark');
  } else {
    document.documentElement.classList.remove('dark');
  }
})
