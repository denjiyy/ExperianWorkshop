import { z } from "zod";

export const registerFormSchema = z
  .object({
    fullName:z.string({required_error:"Field is required"}).min(5),
    username: z.string({ invalid_type_error: "" , required_error:"Field is required" }).min(5),
    email: z.string({ invalid_type_error: "" , required_error:"Field is required" }).email(),
    password: z.string({ required_error:"Field is required" }).regex(/^(?=.*[0-9])(?=.*[a-z])(?=.*[A-Z])(?=.*\W)(?!.* ).{8,16}$/
,{
      message: "Password does not meet the requirements"
    }).max(30),
    confirmPassword: z.string({required_error:"Field is required" }),
    phoneNumber :z.string().regex(/^\+359\d{9}$/,{
      message : "Invalid phone number"
    }),
    address:z.string({required_error:"Field is required"}).min(5),
    dateOfBirth : z.string({required_error:"Field is required", invalid_type_error:"Invalid date"}).date()
  })
  .refine((data) => data.password === data.confirmPassword, {
    message: "Passwords do not match",
    path: ["confirmPassword"],
  });

export type RegisterFormValues = z.infer<typeof registerFormSchema>;
