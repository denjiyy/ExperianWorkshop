import { z } from "zod";

export const loginFormSchema = z.object({
  email: z.string({ invalid_type_error: "",required_error:"Field is required"}).email(),
  password: z.string({ invalid_type_error: "",required_error:"Field is required" }).min(8).max(30),
});

export type LoginFormValues = z.infer<typeof loginFormSchema>;
