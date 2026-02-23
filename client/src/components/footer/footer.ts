export interface Footerlink{
    label:string;
    href:string;
}
export interface FooterSection{
    title:string;
    links:Footerlink[];
}
export const footerSections: FooterSection[]=[
    {
        title:"Kontakt",
        links:[
            {label:"Email", href:"/contact/email"},
            {label:"Telefon", href:"/contact/phone"},
        ],

    },
    {
        title:" O nas",
        links:[
            {label:"AKAI", href:"/about/akai"},
            {label:"Politechnika Poznańska", href:"/about/pp"},
        ],
    },
    {
        title:"Przydatne rzeczy",
        links:[
            {label:"Kierunki studiów", href:"/useful/courses"},
            {label:"Prowadzący zajęcia", href:"/useful/lecturers"},
            {label:"Przedmioty", href:"/useful/subjects"},
            {label:"Szukaj",href:"/useful/search"},
        ],
    },


];
