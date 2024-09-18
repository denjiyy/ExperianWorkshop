import { z } from "zod";

export const loginFormSchema = z.object({
  username: z.string({ invalid_type_error: "",required_error:"Field is required"}).min(5),
  password: z.string({ invalid_type_error: "",required_error:"Field is required" }).min(8).max(30),
});

export type LoginFormValues = z.infer<typeof loginFormSchema>;
