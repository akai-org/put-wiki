import { FooterSections } from './footerSections';
import '@/styles/global.css';
import logo from '@/assets/graphics/footer/logo2.jfif';

export default function Footer() {
  return (
    <footer className="bg-gray-800 text-white py-8">
      <div className="container mx-auto">
        <div className="grid grid-cols-1 md:grid-cols-4 gap-8">
          <div className=" text-center items-center">
            <div className="flex justify-center items-center">
              <img src={logo} className="w-10 h-10" alt="logoOfPolitechnikaPoznanska" />{' '}
            </div>
            <h2>PUT - WIKI</h2>
            Nowoczesne wsparcie dla studentów Politechniki Poznańskiej. Wszystkie informacje w
            jednym miejscu.
          </div>

          {FooterSections.map((section) => (
            <div key={section.id}>
              <h3 className="flex justify-center items-center">{section.title}:</h3>
              <ul>
                {section.links.map((link, index) => (
                  <li key={link.label + index} className="flex justify-center items-center">
                    <a href={link.href}> {link.label} </a>
                  </li>
                ))}
              </ul>
            </div>
          ))}
        </div>
      </div>
    </footer>
  );
}
