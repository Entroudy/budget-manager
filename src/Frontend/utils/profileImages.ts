const PROFILE_IMAGES_KEY = 'budget-manager-user-profile-images'

export const getStoredProfileImage = (userId: string): string | null => {
  if (typeof window === 'undefined') return null
  try {
    const images = JSON.parse(window.localStorage.getItem(PROFILE_IMAGES_KEY) || '{}') as Record<string, string>
    return images[userId] || null
  } catch {
    return null
  }
}

export const storeProfileImage = (userId: string, image: string): void => {
  if (typeof window === 'undefined') return
  try {
    const images = JSON.parse(window.localStorage.getItem(PROFILE_IMAGES_KEY) || '{}') as Record<string, string>
    images[userId] = image
    window.localStorage.setItem(PROFILE_IMAGES_KEY, JSON.stringify(images))
  } catch (error) {
    console.error('Could not store a local profile image copy:', error)
  }
}
