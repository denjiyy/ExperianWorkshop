import { PropsWithChildren, PropsWithRef } from "react";

type PropsWithRefAndChildren<T> = PropsWithChildren<PropsWithRef<T>>;

export type HTMLMainProps = PropsWithRefAndChildren<JSX.IntrinsicElements["main"]>;

export type HTMLDivProps = PropsWithRefAndChildren<PropsWithRef<JSX.IntrinsicElements["div"]>>;

export type HTMLSpanProps = PropsWithRefAndChildren<JSX.IntrinsicElements["span"]>;

export type HTMLSectionProps = PropsWithRefAndChildren<JSX.IntrinsicElements["section"]>;

export type HTMLNavProps = PropsWithRefAndChildren<JSX.IntrinsicElements["nav"]>;

export type HTMLHeaderProps = PropsWithRefAndChildren<JSX.IntrinsicElements["header"]>;

export type HTMLFooterProps = PropsWithRefAndChildren<JSX.IntrinsicElements["footer"]>;

export type HTMLButtonProps = PropsWithRefAndChildren<JSX.IntrinsicElements["button"]>;

export type HTMLAnchorProps = PropsWithRefAndChildren<JSX.IntrinsicElements["a"]>;

export type HTMLInputProps = PropsWithRefAndChildren<JSX.IntrinsicElements["input"]>;

export type HTMLTextAreaProps = PropsWithRefAndChildren<JSX.IntrinsicElements["textarea"]>;

export type HTMLFormProps = PropsWithRefAndChildren<JSX.IntrinsicElements["form"]>;

export type HTMLHeadingPropsH1 = PropsWithRefAndChildren<JSX.IntrinsicElements["h1"]>;
export type HTMLHeadingPropsH3 = PropsWithRefAndChildren<JSX.IntrinsicElements["h3"]>;

export type HTMLHeadingPropsH4 = PropsWithRefAndChildren<JSX.IntrinsicElements["h4"]>;

export type HTMLHeadingPropsBody = PropsWithRefAndChildren<JSX.IntrinsicElements["h5"]>;



export type HTMLParagraphProps = PropsWithRefAndChildren<JSX.IntrinsicElements["p"]>;

export type HTMLSVGProps = PropsWithRefAndChildren<JSX.IntrinsicElements["svg"]>;

export type HTMLImageProps = PropsWithRefAndChildren<JSX.IntrinsicElements["img"]>;

export type HTMLLabelProps = PropsWithRefAndChildren<JSX.IntrinsicElements["label"]>;

export type HTMLAsideProps = PropsWithRefAndChildren<JSX.IntrinsicElements["aside"]>;
