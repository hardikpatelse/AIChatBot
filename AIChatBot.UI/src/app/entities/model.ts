export interface Model {
    id: string
    name: string
    company?: string
    logoUrl?: string
    description?: string
    referralSource?: string
    referenceLink?: string
    supportedModes: string[]
}
