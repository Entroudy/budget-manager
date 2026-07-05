export default defineI18nConfig(async () => {
  const enMessages = await import('@/locales/en.json');
  const huMessages = await import('@/locales/hu.json');

  return {
    legacy: false,
    locale: 'hu',
    fallbackLocale: 'en',
    messages: {
      en: enMessages.default,
      hu: huMessages.default
    }
  };
});
