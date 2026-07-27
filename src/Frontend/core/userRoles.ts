import type { UserRole } from '~/models/UserModel'

export const USER_ROLES: UserRole[] = ['User', 'Admin']

export const DEFAULT_USER_ROLE: UserRole = 'User'

export const getUserRoleLabelKey = (role?: string) => {
  switch (role) {
    case 'Admin':
      return 'administrator'
    case 'User':
      return 'standard_user'
    default:
      return 'unknown_role'
  }
}

export const getUserRoleBadgeClass = (role?: string) => {
  switch (role) {
    case 'Admin':
      return 'bg-amber-100 text-amber-700 dark:bg-amber-900 dark:text-amber-200'
    case 'User':
      return 'bg-primary/10 text-primary dark:bg-primary/20 dark:text-primary'
    default:
      return 'bg-gray-100 text-gray-700 dark:bg-gray-700 dark:text-gray-200'
  }
}
