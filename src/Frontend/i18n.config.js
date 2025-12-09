export default defineI18nConfig(async () => {
  const huMessages = await import('@/locales/hu.json');
  const enMessages = await import('@/locales/en.json');

  return {
    legacy: false,
    locale: 'hu',
    messages: {
      hu: huMessages.default,
      en: enMessages.default
    }
  };
});
