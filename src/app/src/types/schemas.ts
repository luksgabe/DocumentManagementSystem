import z from "zod";

export const shareSchema = z.object({
    targetType: z
        .number()
        .int()
        .refine((v) => v === 1 || v === 2, "Invalid target type"),

    targetValue: z
        .string()
        .min(3, "Value must contain at least 3 characters")
        .email("Must be a valid email")
        .or(z.string().min(1, "Required")),

    permission: z
        .number()
        .int()
        .refine((v) => v >= 0 && v <= 3, "Invalid permission"),
});

export type ShareForm = z.infer<typeof shareSchema>;