import link from 'next/link';
import {footerSections} from '@/config/footerSections';


const Footer=() => {

return(
    <footer className="bg-gray-800 text-white py-8">
        <div className="container mx-auto">
            <div className="grid grid-cols-2 md:grid-cols-4 gap-8">
                {footerSections.map((section) => (
                    <div key={section.title}>
                        <h3>{section.title}</h3>
                        <ul>
                            {section.links.map((link) => (
                              <li key={link.href}>
                                <Link href={link.href}> {link.label} </Link>
                              </li>  
                            ))};
                        </ul>
                    </div>
                ))}
            </div>
        </div>
   
   </footer>
)

};



footer{

}