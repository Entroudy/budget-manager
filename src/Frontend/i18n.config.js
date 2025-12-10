export default defineI18nConfig(async () => {
  const enMessages = await import('@/locales/en.json');

  return {
    legacy: false,
    locale: 'en',
    messages: {
      en: enMessages.default
    }
  };
});
